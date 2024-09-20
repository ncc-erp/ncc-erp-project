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
using System.Linq.Expressions;
using Microsoft.AspNetCore.SignalR;
using System;
using ProjectManagement.Services.ProjectUserBill;
using Abp.Linq.Extensions;
using Abp.Extensions;

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

            var quser = _workScope.GetAll<User>()
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
                       })
                       .ToList();

            return quser;
        }

        public async Task<long> GetActiveReportId()
        {
            return await _workScope.GetAll<PMReport>()
                .Where(s => s.IsActive == true)
                .OrderByDescending(s => s.Id)
                .Select(s => s.Id).FirstOrDefaultAsync();
        }

        public async Task<List<GetProjectUserBillDto>> GetAllByProject(GetAllProjectUserBillDto input)
        {
            var isViewRate = await IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View);

            var query = _workScope.GetAll<Entities.ProjectUserBill>()
                .Where(x => x.ProjectId == input.ProjectId)
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
                    HeadCount = x.HeadCount,
                    StartTime = x.StartTime.Date,
                    EndTime = x.EndTime.Value.Date,
                    Note = x.Note,
                    shadowNote = x.shadowNote,
                    isActive = x.isActive,
                    isExpose = x.isExpose,
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
                    ChargeType = x.ChargeType ?? x.Project.ChargeType,
                    CreationTime = x.CreationTime,
                    LinkCV = x.LinkCV,

                    LinkedResources = x.LinkedResources
                        .Select(lr => new GetUserInfo
                        {
                            Id = lr.UserId,
                            EmailAddress = lr.User.EmailAddress,
                            UserName = lr.User.UserName,
                            AvatarPath = lr.User.AvatarPath ?? "",
                            UserType = lr.User.UserType,
                            PositionId = lr.User.PositionId,
                            PositionColor = lr.User.Position.Color,
                            PositionName = lr.User.Position.ShortName,
                            UserLevel = lr.User.UserLevel,
                            BranchColor = lr.User.Branch.Color,
                            BranchDisplayName = lr.User.Branch.DisplayName,
                            IsActive = lr.User.IsActive,
                            FullName = lr.User.FullName,
                        }).ToList()
                });


            var result = query.WhereIf(input.ChargeStatusFilter != ChargeStatusFilter.All, x => x.isActive == (input.ChargeStatusFilter == ChargeStatusFilter.IsCharge))
                        .WhereIf(input.ChargeRoleFilter != null && input.ChargeRoleFilter.Any(), x => input.ChargeRoleFilter.Contains(x.BillRole))
                        .ApplySearch(input.SearchText).OrderByDescending(x => x.CreationTime)
                        .ToList();

            if (input.LinkedResourcesFilter != null && input.LinkedResourcesFilter.Any())
            {
                result = result.Where(x => x.LinkedResources.Any(lr => input.LinkedResourcesFilter.Contains(lr.Id))).ToList();
            }
            var totalHeadCount = result.Sum(x => x.HeadCount);

            result.ForEach(item => item.totalHeadCount = totalHeadCount);
            return result;
        }

        public async Task<GetProjectUserBillDto> GetProjectUserBillById(long projectUserBillId)
        {
            var isViewRate = await IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabBillInfo_Rate_View);

            return _workScope.GetAll<Entities.ProjectUserBill>()
                .Where(x => x.Id == projectUserBillId)
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
                    HeadCount = x.HeadCount,
                    StartTime = x.StartTime.Date,
                    EndTime = x.EndTime.Value.Date,
                    Note = x.Note,
                    shadowNote = x.shadowNote,
                    isActive = x.isActive,
                    isExpose = x.isExpose,
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
                    ChargeType = x.ChargeType ?? x.Project.ChargeType,
                    CreationTime = x.CreationTime,
                    LinkCV = x.LinkCV,

                    LinkedResources = x.LinkedResources
                        .Select(lr => new GetUserInfo
                        {
                            Id = lr.UserId,
                            EmailAddress = lr.User.EmailAddress,
                            UserName = lr.User.UserName,
                            AvatarPath = lr.User.AvatarPath ?? "",
                            UserType = lr.User.UserType,
                            PositionId = lr.User.PositionId,
                            PositionColor = lr.User.Position.Color,
                            PositionName = lr.User.Position.ShortName,
                            UserLevel = lr.User.UserLevel,
                            BranchColor = lr.User.Branch.Color,
                            BranchDisplayName = lr.User.Branch.DisplayName,
                            IsActive = lr.User.IsActive,
                            FullName = lr.User.FullName,
                        }).ToList()
                }).FirstOrDefault();
        }
         
        public async Task<List<LinkedResourceInfoDto>> GetAllLinkedResourcesByProject(long projectId)
        {
            var result = await _workScope.GetAll<LinkedResource>()
                .Where(x => x.ProjectUserBill.ProjectId == projectId)
                .Where(x => x.User != null)
                .Select(x => new LinkedResourceInfoDto
                {
                    Id = x.User.Id,
                    EmailAddress = x.User.EmailAddress,
                    FullName = x.User.FullName
                })
                .Distinct()
                .ToListAsync();

            result = result.OrderBy(x => x.EmailAddress).ToList();

            return result;
        }

        public async Task<List<string>> GetAllChargeRoleByProject(long projectId)
        {
            var result = _workScope.GetAll<Entities.ProjectUserBill>()
                .Where(x => x.ProjectId == projectId)
                .Select(x => x.BillRole)
                .Distinct()
                .ToListAsync();

            return await result;
        }

        public async Task<List<UserDto>> GetAllUser(bool onlyStaff, long projectId, long? currentUserId, bool isIncludedUserInPUB)
        {
            var listPUBIds = await _workScope.GetAll<ProjectManagement.Entities.ProjectUserBill>()
                .Where(x => x.ProjectId == projectId)
                .Select(x => x.UserId)
                .ToListAsync();

            if (currentUserId.HasValue)
                listPUBIds = listPUBIds.Where(x => x != currentUserId).ToList();

            var query = _workScope.GetAll<User>()
                .Where(x => x.UserType != UserType.Vendor)
                .Where(x => x.UserType != UserType.FakeUser)
                .Where(x => onlyStaff ? x.UserType != UserType.Internship : true)
                .Where(x => !isIncludedUserInPUB? !listPUBIds.Contains(x.Id) : true)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Name = u.Name,
                    Surname = u.Surname,
                    EmailAddress = u.EmailAddress,
                    FullName = u.FullName,
                    AvatarPath = u.AvatarPath,
                    UserType = u.UserType,
                    UserLevel = u.UserLevel,
                    Branch = u.BranchOld,
                    PositionId = u.PositionId,
                    IsActive = u.IsActive,
                    UserSkills = u.UserSkills.Select(x => new UserSkillDto
                    {
                        SkillId = x.SkillId,
                        SkillName = x.Skill.Name
                    }).ToList()
                })
                .Distinct()
                .ToList();

            query = query.OrderByDescending(x => x.IsActive).ToList();
            return query;
        }

        public async Task<List<BillAccountDto>> GetAllBillAccount()
        {
            return await _workScope.GetAll<Entities.ProjectUserBill>()
                .Select(p => new BillAccountDto()
                {
                    EmailAddress = p.User.EmailAddress,
                    UserId = p.User.Id
                })
                .Distinct()
                .ToListAsync();
        }

        public async Task<List<LinkedResource>> AddLinkedResources(LinkedResourcesDto input)
        {
            var listLinkedResources = new List<LinkedResource>();

            if (input.UserIds.Count == 0) return listLinkedResources;

            ValidateProjectUserBill(input.ProjectUserBillId);

            foreach (var userId in input.UserIds)
            {
                ValidateUser(userId);

                var existingLinkedResource = await _workScope.GetAll<LinkedResource>()
                    .Where(lr => lr.UserId == userId && lr.ProjectUserBillId == input.ProjectUserBillId)
                    .FirstOrDefaultAsync();

                if (existingLinkedResource != null)
                {
                    existingLinkedResource.IsDeleted = false;
                    await _workScope.UpdateAsync(existingLinkedResource);
                    listLinkedResources.Add(existingLinkedResource);
                }
                else
                {
                    var newLinkedResource = new LinkedResource
                    {
                        UserId = userId,
                        ProjectUserBillId = input.ProjectUserBillId,
                    };

                    await _workScope.InsertAsync(newLinkedResource);
                    listLinkedResources.Add(newLinkedResource);
                }
            }

            return listLinkedResources;
        }

        public async Task LinkOneLinkedResource(LinkedResourceDto input)
        {
            ValidateProjectUserBill(input.ProjectUserBillId);

            ValidateUser(input.UserId);


            var newLinkedResource = new LinkedResource
            {
                UserId = input.UserId,
                ProjectUserBillId = input.ProjectUserBillId,
            };

            await _workScope.InsertAsync(newLinkedResource);
        }

        public async Task RemoveLinkedResource(LinkedResourcesDto input)
        {
            ValidateProjectUserBill(input.ProjectUserBillId);

            foreach (var userId in input.UserIds)
            {
                ValidateUser(userId);

                var existingLinkedResource = await _workScope.GetAll<LinkedResource>()
                    .Where(lr => lr.UserId == userId && lr.ProjectUserBillId == input.ProjectUserBillId)
                    .FirstOrDefaultAsync();

                if (existingLinkedResource != null)
                    await _workScope.DeleteAsync(existingLinkedResource);
            }
        }

        private void ValidateUser(long userId)
        {
            var existedUser = _workScope.GetAll<User>()
                .Where(s => s.Id == userId)
                .Any();

            if (!existedUser)
                throw new UserFriendlyException($"User with Id {userId} does not exist!");
        }

        private void ValidateProjectUserBill(long projectUserBillId)
        {
            var isExist = _workScope.GetAll<Entities.ProjectUserBill>()
                .Where(s => s.Id == projectUserBillId)
                .Any();

            if (!isExist)
                throw new UserFriendlyException($"ProjectUserBill with Id {projectUserBillId} does not exist!");
        }

    }
}