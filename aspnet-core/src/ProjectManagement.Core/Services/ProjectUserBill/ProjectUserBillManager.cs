using Abp.Application.Services;
using NccCore.IoC;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using System.Linq;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Microsoft.EntityFrameworkCore;
using NccCore.Paging;
using NccCore.Extension;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Collections.Generic;
using ProjectManagement.Services.ProjectUserBill.Dto;
using Abp.UI;
using Abp.Domain.Uow;
using Abp;

namespace ProjectManagement.Services.ProjectUserBills
{
    public class ProjectUserBillManager : ApplicationService
    {
        private readonly IWorkScope _workScope;

        public ProjectUserBillManager(IWorkScope workScope)
        {
            _workScope = workScope;
        }
        public IQueryable<ProjectManagement.Entities.ProjectUserBill> GetSubProjectBills(long parentProjectId)
        {
            var query = from p in _workScope.GetAll<Project>()
                        where p.ParentInvoiceId == parentProjectId
                        join pub in _workScope.GetAll<ProjectManagement.Entities.ProjectUserBill>()
                         on p.Id equals pub.ProjectId
                        select pub;
            return query.OrderBy(p => p.Project.Name).ThenBy(p => p.User.EmailAddress);
        }
    
        public async Task<List<GetAllResourceDto>> QueryAllResource(bool isVendor)
        {
            // get current user and view user level permission
            // if user level = intern => all show no matter the permission
            var listLoginUserPM = _workScope.GetAll<ProjectUser>()
                .Where(pu => pu.Project.Status != ProjectStatus.Closed
                    && (pu.Status == ProjectUserStatus.Present && pu.AllocatePercentage > 0
                    || pu.Status == ProjectUserStatus.Future))
                .Where(pu => pu.UserId == AbpSession.UserId.GetValueOrDefault() && pu.ProjectRole == 0 || pu.Project.PMId == AbpSession.UserId.GetValueOrDefault()
                    ).Select(pu => pu.Id);

            var activeReportId = await GetActiveReportId();
            var quser = await _workScope.GetAll<User>()
                       .Where(x => x.IsActive)
                       .Where(x => x.UserType != UserType.FakeUser)
                       .Where(u => isVendor ? u.UserType == UserType.Vendor : u.UserType != UserType.Vendor)
                       .Select(x => new GetAllResourceDto
                       {
                           UserId = x.Id,
                           UserType = x.UserType,
                           FullName = x.Name + " " + x.Surname,
                           NormalFullName = x.Surname + " " + x.Name,
                           EmailAddress = x.EmailAddress,
                           Branch = x.BranchOld,
                           BranchColor = x.Branch.Color,
                           BranchDisplayName = x.Branch.DisplayName,
                           BranchId = x.BranchId,
                           PositionId = x.PositionId,
                           PositionColor = x.Position.Color,
                           PositionName = x.Position.ShortName,
                           UserLevel = x.UserLevel >= UserLevel.Intern_0
                                && x.UserLevel <= UserLevel.Intern_3 ? x.UserLevel :
                                _workScope.GetAll<ProjectUser>().Any(pu => pu.UserId == x.Id
                                && listLoginUserPM.Contains(pu.Id)) ? x.UserLevel : default(UserLevel?),
                           AvatarPath = x.AvatarPath,
                           StarRate = x.StarRate,
                           UserSkills = x.UserSkills.Select(s => new UserSkillDto
                           {
                               UserId = s.UserId,
                               SkillId = s.SkillId,
                               SkillName = s.Skill.Name,
                               SkillRank = s.SkillRank
                           }).ToList(),

                           PlanProjects = x.ProjectUsers
                           .Where(pu => pu.Status == ProjectUserStatus.Future)
                           .Where(pu => pu.Project.Status != ProjectStatus.Closed)
                           .Where(pu => pu.PMReportId == activeReportId)
                           .Select(pu => new ProjectOfUserDto
                           {
                               Id = pu.Id,
                               ProjectId = pu.ProjectId,
                               ProjectName = pu.Project.Name,
                               ProjectRole = pu.ProjectRole,
                               PmName = pu.Project.PM.Name,
                               StartTime = pu.StartTime,
                               IsPool = pu.IsPool,
                               AllocatePercentage = pu.AllocatePercentage,
                               ProjectType = pu.Project.ProjectType,
                               ProjectCode = pu.Project.Code
                           })
                           .ToList(),

                           WorkingProjects = x.ProjectUsers
                            .Where(s => s.Status == ProjectUserStatus.Present
                            && s.AllocatePercentage > 0
                            && s.Project.Status != ProjectStatus.Closed)
                            .Select(pu => new ProjectOfUserDto
                            {
                                Id = pu.Id,
                                ProjectId = pu.ProjectId,
                                ProjectName = pu.Project.Name,
                                ProjectRole = pu.ProjectRole,
                                ProjectStatus = pu.Project.Status,
                                PmName = pu.Project.PM.Name,
                                StartTime = pu.StartTime,
                                IsPool = pu.IsPool,
                                ProjectType = pu.Project.ProjectType,
                                ProjectCode = pu.Project.Code
                            })
                           .ToList(),

                       }).ToListAsync();

            return quser ;
        }

        public async Task<long> GetActiveReportId()
        {
            return await _workScope.GetAll<PMReport>()
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.Id)
                .Select(s => s.Id).FirstOrDefaultAsync();
        }

        public async Task<List<GetAllUserDto>> GetUserBillAccountsByAccount(long accountId, long projectId)
        {
            await CheckResourceInProject(accountId, projectId);

            var existedPUBLU = await _workScope.GetAll<ProjectUserBillAccount>()
                .Where(x => x.UserBillAccountId == accountId && x.ProjectId == projectId)
                .Select(x => x.UserId)
                .ToListAsync();

            var listResult = await _workScope.GetAll<User>()
                .Where(u => existedPUBLU.Contains(u.Id))
                .Select(u => new GetAllUserDto
                {
                    Id = u.Id,
                    EmailAddress = u.EmailAddress,
                    UserName = u.UserName,
                    AvatarPath = u.AvatarPath == null ? "" : u.AvatarPath,
                    UserType = u.UserType,
                    PositionId = u.PositionId,
                    PositionColor = u.Position.Color,
                    PositionName = u.Position.ShortName,
                    UserLevel = u.UserLevel,
                    Branch = u.BranchOld,
                    BranchColor = u.Branch.Color,
                    BranchDisplayName = u.Branch.DisplayName,
                    IsActive = u.IsActive,
                    FullName = u.Name + " " + u.Surname,
                    CreationTime = u.CreationTime,
                    UserSkills = u.UserSkills.Select(s => new UserSkillDto
                    {
                        UserId = u.Id,
                        SkillId = s.SkillId,
                        SkillName = s.Skill.Name
                    }).ToList(),
                    WorkingProjects = u.ProjectUsers
                            .Where(s => s.Status == ProjectUserStatus.Present && s.AllocatePercentage > 0)
                            .Where(x => x.Project.Status != ProjectStatus.Potential && x.Project.Status != ProjectStatus.Closed)
                            .Select(p => new WorkingProjectDto
                            {
                                ProjectName = p.Project.Name,
                                ProjectRole = p.ProjectRole,
                                StartTime = p.StartTime,
                                IsPool = p.IsPool

                            }).ToList(),
                })
                .ToListAsync();

            return listResult;
        }
   
        public async Task CheckResourceInProject(long accountId, long projectId)
        {
            var existedPU = await _workScope.GetAll<ProjectManagement.Entities.ProjectUserBill>()
                .Where(x => x.UserId == accountId && x.ProjectId == projectId)
                .FirstOrDefaultAsync();

            if (existedPU == null)
                throw new UserFriendlyException($"BillAccount(User) {accountId} is not working in Project {projectId}!");
        }

        public async Task<ProjectUserBillAccount> AddProjectUserBillAccount(CreateProjectUserBillLinkDto input)
        {
            await ValidateProjectUserBillAccount(input.UserId, input.ProjectId, input.UserBillAccountId);

            var pUBA = new ProjectUserBillAccount();

            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
            {
                pUBA = await _workScope.GetAll<ProjectUserBillAccount>()
                 .Where(s => s.UserBillAccountId == input.UserBillAccountId && s.UserId == input.UserId && s.ProjectId == input.ProjectId)
                 .FirstOrDefaultAsync();
            }

            if (pUBA != null && !pUBA.IsDeleted)
            {
                throw new UserFriendlyException("This UserBillAccount already exists!");
            } 
            else if (pUBA != null && pUBA.IsDeleted)
            {
                pUBA.IsDeleted = false;
                await _workScope.UpdateAsync(pUBA);
                return pUBA;
            } 
            else if (pUBA == null)
            {
                var newBA = new ProjectUserBillAccount
                {
                    UserId = input.UserId,
                    ProjectId = input.ProjectId,
                    UserBillAccountId = input.UserBillAccountId,
                };

                await _workScope.InsertAsync(newBA);    
                return newBA;
            }

            return null;
        }

        public async Task<bool> RemoveProjectUserBillAccount(GetProjectUserBillLinkDto input)
        {
            await ValidateProjectUserBillAccount(input.UserId, input.ProjectId, input.UserBillAccountId);

            var pUBA = await _workScope.GetAll<ProjectUserBillAccount>()
               .Where(s => s.UserBillAccountId == input.UserBillAccountId && s.UserId == input.UserId && s.ProjectId == input.ProjectId)
               .FirstOrDefaultAsync();

            if (pUBA == null)
                throw new UserFriendlyException("This BillAccountUser does not exist!");
            else
                await _workScope.DeleteAsync(pUBA);

            return true;
        }

        private async Task ValidateProjectUserBillAccount(long userId, long projectId, long billAccountId)
        {
            var existedUser = await _workScope.GetAll<User>()
              .Where(s => s.Id == userId)
              .FirstOrDefaultAsync();
            if (existedUser == null)
                throw new UserFriendlyException($"User with Id {userId} does not exist!");

            var existedPUB = await _workScope.GetAll<ProjectManagement.Entities.ProjectUserBill>()
               .Where(x => x.UserId == billAccountId && x.ProjectId == projectId)
               .FirstOrDefaultAsync();
            if (existedPUB == null)
                throw new UserFriendlyException($"BillAccount(User) {billAccountId} is not working in Project {projectId}!");
        }
      
    }
}
