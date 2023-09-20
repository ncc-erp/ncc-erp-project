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
using System.Linq.Dynamic.Core;

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

        public async Task<List<GetProjectUserBillDto>> GetAllByProject(long projectId)
        {
            var isViewRate = await IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View);

            var existedPUBLUIds = await _workScope.GetAll<ProjectUserBillAccount>()
              .Where(x => x.ProjectId == projectId)
              .Select(x => x.UserId)
              .ToListAsync();

            var allPUBA = _workScope.GetAll<ProjectUserBillAccount>();
            var allUser = _workScope.GetAll<User>();

            var query = await _workScope.GetAll<ProjectManagement.Entities.ProjectUserBill>()
                 .Where(x => x.ProjectId == projectId)
                 .Select(x => new GetProjectUserBillDto
                 {
                     Id = x.Id,
                     UserId = x.UserId,
                     UserName = x.User.Name,
                     ProjectId = x.ProjectId,
                     ProjectName = x.Project.Name,
                     AccountName = x.AccountName,
                     BillRole = x.BillRole,
                     BillRate = isViewRate ? x.BillRate : 0,
                     StartTime = x.StartTime.Date,
                     EndTime = x.EndTime.Value.Date,
                     //CurrencyName = x.Project.Currency.Name,
                     Note = x.Note,
                     shadowNote = x.shadowNote,
                     isActive = x.isActive,
                     AvatarPath = x.User.AvatarPath,
                     FullName = x.User.FullName,
                     Branch = x.User.BranchOld,
                     BranchColor = x.User.Branch.Color,
                     BranchDisplayName = x.User.Branch.DisplayName,
                     PositionId = x.User.PositionId,
                     PositionName = x.User.Position.ShortName,
                     PositionColor = x.User.Position.Color,
                     EmailAddress = x.User.EmailAddress,
                     UserType = x.User.UserType,
                     UserLevel = x.User.UserLevel,
                     ChargeType = x.ChargeType.HasValue ? x.ChargeType : x.Project.ChargeType,
                     CreationTime = x.CreationTime,

                     LinkedResources = (from pu in allPUBA
                                        join us in allUser on pu.UserId equals us.Id
                                        where pu.UserBillAccountId == x.UserId && existedPUBLUIds.Contains(pu.UserId)
                                        select new GetAllUserDto
                                        {
                                            Id = us.Id, // Set the appropriate properties you want from us
                                            EmailAddress = us.EmailAddress,
                                            UserName = us.UserName,
                                            AvatarPath = us.AvatarPath == null ? "" : us.AvatarPath,
                                            UserType = us.UserType,
                                            PositionId = us.PositionId,
                                            PositionColor = us.Position.Color,
                                            PositionName = us.Position.ShortName,
                                            UserLevel = us.UserLevel,
                                            Branch = us.BranchOld,
                                            BranchColor = us.Branch.Color,
                                            BranchDisplayName = us.Branch.DisplayName,
                                            IsActive = us.IsActive,
                                            FullName = us.Name + " " + us.Surname,
                                            CreationTime = us.CreationTime,
                                            UserSkills = us.UserSkills.Select(s => new UserSkillDto
                                            {
                                                UserId = us.Id,
                                                SkillId = s.SkillId,
                                                SkillName = s.Skill.Name
                                            }).ToList(),
                                            WorkingProjects = us.ProjectUsers
                                                .Where(s => s.Status == ProjectUserStatus.Present && s.AllocatePercentage > 0)
                                                .Where(x => x.Project.Status != ProjectStatus.Potential && x.Project.Status != ProjectStatus.Closed)
                                                .Select(p => new WorkingProjectDto
                                                {
                                                    ProjectName = p.Project.Name,
                                                    ProjectRole = p.ProjectRole,
                                                    StartTime = p.StartTime,
                                                    IsPool = p.IsPool
                                                }).ToList(),
                                        }).ToList()

                 })
                 .OrderByDescending(x => x.CreationTime)
                 .ToListAsync();

            return query;
        }

        public async Task<List<ProjectUserBillAccount>> AddProjectUserBillAccounts(ProjectUserBillAccountsDto input)
        {
            var listPUBA = new List<ProjectUserBillAccount>();
            foreach (var userId in input.UserIds)
            {
                await ValidateProjectUserBillAccount(userId, input.ProjectId, input.BillAccountId);

                var pUBA = new ProjectUserBillAccount();

                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.SoftDelete))
                {
                    pUBA = await _workScope.GetAll<ProjectUserBillAccount>()
                     .Where(s => s.UserBillAccountId == input.BillAccountId && s.UserId == userId && s.ProjectId == input.ProjectId)
                     .FirstOrDefaultAsync();
                }

                if (pUBA != null && !pUBA.IsDeleted)
                {
                    throw new UserFriendlyException($"UserBillAccount {pUBA.Id} already exists!");
                }
                else if (pUBA != null && pUBA.IsDeleted)
                {
                    pUBA.IsDeleted = false;
                    await _workScope.UpdateAsync(pUBA);
                    listPUBA.Add(pUBA);
                }
                else if (pUBA == null)
                {
                    var newBA = new ProjectUserBillAccount
                    {
                        UserId = userId,
                        ProjectId = input.ProjectId,
                        UserBillAccountId = input.BillAccountId,
                    };

                    await _workScope.InsertAsync(newBA);
                    listPUBA.Add(newBA);
                }
            }

            return listPUBA;
        }

        public async Task RemoveProjectUserBillAccount(ProjectUserBillAccountsDto input)
        {
            foreach (var userId in input.UserIds)
            {
                await ValidateProjectUserBillAccount(userId, input.ProjectId, input.BillAccountId);

                var pUBA = await _workScope.GetAll<ProjectUserBillAccount>()
                   .Where(s => s.UserBillAccountId == input.BillAccountId && s.UserId == userId && s.ProjectId == input.ProjectId)
                   .FirstOrDefaultAsync();

                if (pUBA != null)
                    await _workScope.DeleteAsync(pUBA);
            }
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
