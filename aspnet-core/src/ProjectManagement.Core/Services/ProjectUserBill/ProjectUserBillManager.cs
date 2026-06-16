using Abp.Application.Services;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.IoC;
using NccCore.Uitls;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.Services.ProjectUserBill.Dto;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.UploadFilesService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectUserBills
{
    public class ProjectUserBillManager : ApplicationService
    {
        private readonly IWorkScope _workScope;
        private readonly UploadFileService _uploadFileService;

        public ProjectUserBillManager(IWorkScope workScope, UploadFileService uploadFileService)
        {
            _workScope = workScope;
            _uploadFileService = uploadFileService;
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

        public async Task<List<GetAllResourceDto>> QueryAllResource(bool isVendor = false, bool showVendor = false)
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
                       .WhereIf(!showVendor, u => isVendor ? u.UserType == UserType.Vendor : u.UserType != UserType.Vendor)
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
                               ProjectCode = pu.Project.Code,
                               WorkingType = pu.WorkingType
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
                    AccountName = x.AccountName.IsEmpty() ? x.User.UserName : x.AccountName,
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
                    UserSkills = x.BillUserSkills.Select(us => new BillUserSkillDto
                    {
                        SkillId = us.SkillId,
                        SkillName = us.Skill.Name,
                        SkillRank = us.SkillRank,
                        SkillNote = us.Note
                    }).ToList(),
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
                            Contribute = lr.Contribute
                        }).ToList(),
                    AccountAssets = x.AccountAssets
                        .Select(ar => new AccountAssetDto
                        {
                            Id = ar.Id,
                            ProjectUserBillId = ar.ProjectUserBillId,
                            ProjectAssetId = ar.ProjectAssetId,
                            ProjectAssetTypeId = ar.ProjectAsset.ProjectAssetTypeId,
                            ProjectAssetTypeName = ar.ProjectAsset.ProjectAssetType != null ? ar.ProjectAsset.ProjectAssetType.Name : string.Empty,
                            AccountTypeId = ar.ProjectAsset.AccountTypeId,
                            AccountTypeName = ar.ProjectAsset.AccountType != null ? ar.ProjectAsset.AccountType.Name : string.Empty,
                            AccountAssetCreatorId = ar.ProjectAsset.AccountAssetCreatorId,
                            AccountAssetCreatorName = ar.ProjectAsset.AccountAssetCreator != null ? ar.ProjectAsset.AccountAssetCreator.Name : string.Empty,
                            TypeLoginId = ar.ProjectAsset.TypeLoginId,
                            TypeLoginName = ar.ProjectAsset.TypeLogin != null ? ar.ProjectAsset.TypeLogin.Name : string.Empty,
                            AssetName = ar.ProjectAsset.AssetName,
                        }).ToList(),
                    SkillNote = x.BillUserSkills.Select(s => s.Note).FirstOrDefault() ?? ""
                });


            /*var result = query.WhereIf(input.ChargeStatusFilter != ChargeStatusFilter.All, x => x.isActive == (input.ChargeStatusFilter == ChargeStatusFilter.IsCharge))
                        .WhereIf(input.ChargeRoleFilter != null && input.ChargeRoleFilter.Any(), x => input.ChargeRoleFilter.Contains(x.BillRole))
                        .ApplySearch(input.SearchText).OrderByDescending(x => x.CreationTime)
                        .ToList();*/

            var result = query.ToList();
            result = result.OrderByDescending(x => x.CreationTime).ToList();

            if (input.ChargeStatusFilter != null  && input.ChargeStatusFilter  != ChargeStatusFilter.All)
            {
                bool isCharge = input.ChargeStatusFilter == ChargeStatusFilter.IsCharge;
                result = result.Where(x => x.isActive == isCharge).ToList();
            }

            if (input.ChargeRoleFilter != null && input.ChargeRoleFilter.Any())
            {
                result = result.Where(x => input.ChargeRoleFilter.Contains(x.BillRole)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(input.SearchText))
            {
                var lowerSearch = input.SearchText.Trim().ToLower();
                result = result.Where(x =>
                    (!string.IsNullOrEmpty(x.UserName) && x.UserName.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(x.EmailAddress) && x.EmailAddress.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(x.FullName) && x.FullName.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(x.BillAccountName) && x.BillAccountName.ToLower().Contains(lowerSearch)) ||
                    (!string.IsNullOrEmpty(x.Note) && x.Note.ToLower().Contains(lowerSearch)) ||

                    (x.HeadCount.ToString().Contains(lowerSearch)) ||
                    (!input.IsAccountInfoTab && x.BillRate.ToString().Contains(lowerSearch)) ||
                    (x.UserSkills != null && x.UserSkills.Any(us =>
                        !string.IsNullOrEmpty(us.SkillName) && us.SkillName.ToLower().Contains(lowerSearch)
                    )) 
                ).ToList();
            }

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
                            Contribute = lr.Contribute
                        }).ToList(),
                    AccountAssets = x.AccountAssets
                        .Select(ar => new AccountAssetDto
                        {
                            Id = ar.Id,
                            ProjectUserBillId = ar.ProjectUserBillId,
                            ProjectAssetId = ar.ProjectAssetId,
                            ProjectAssetTypeId = ar.ProjectAsset.ProjectAssetTypeId,
                            ProjectAssetTypeName = ar.ProjectAsset.ProjectAssetType != null ? ar.ProjectAsset.ProjectAssetType.Name : string.Empty,
                            AccountTypeId = ar.ProjectAsset.AccountTypeId,
                            AccountTypeName = ar.ProjectAsset.AccountType != null ? ar.ProjectAsset.AccountType.Name : string.Empty,
                            AccountAssetCreatorId = ar.ProjectAsset.AccountAssetCreatorId,
                            AccountAssetCreatorName = ar.ProjectAsset.AccountAssetCreator != null ? ar.ProjectAsset.AccountAssetCreator.Name : string.Empty,
                            TypeLoginId = ar.ProjectAsset.TypeLoginId,
                            TypeLoginName = ar.ProjectAsset.TypeLogin != null ? ar.ProjectAsset.TypeLogin.Name : string.Empty,
                            AssetName = ar.ProjectAsset.AssetName,
                        }).ToList(),
                    UserSkills = x.BillUserSkills.Select(us => new BillUserSkillDto
                    {
                        SkillId = us.SkillId,
                        SkillName = us.Skill.Name,
                        SkillRank = us.SkillRank,
                        SkillNote = us.Note
                    }).ToList(),
                    SkillNote = x.BillUserSkills.Select(s => s.Note).FirstOrDefault() ?? ""
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

        public async Task<GetUserInfo> LinkOneLinkedResource(LinkedResourceDto input)
        {
            var projectUserBill = _workScope.GetAll<Entities.ProjectUserBill>()
                  .Where(s => s.Id == input.ProjectUserBillId)
                  .FirstOrDefault();

            if (projectUserBill == null)
                throw new UserFriendlyException($"ProjectUserBill with Id {input.ProjectUserBillId} does not exist!");

            ValidateUser(input.UserId);

            if (await CheckTotalContribute(input.ProjectUserBillId, input.Contribute, input.UserId))
            {
                throw new UserFriendlyException("Total contribution cannot exceed 100%.");
            }

            var newLinkedResource = new LinkedResource
            {
                UserId = input.UserId,
                ProjectUserBillId = input.ProjectUserBillId,
                Contribute = input.Contribute
            };

            var linkedId = await _workScope.InsertAndGetIdAsync(newLinkedResource);
            if (input.PmReportId.HasValue)
            {
                var history = new WeeklyContributionHistory
                {
                    ProjectUserBillId = input.ProjectUserBillId,
                    PMReportId = input.PmReportId.Value,
                    Contribute = input.Contribute,
                    UserId = input.UserId,
                    ProjectId = projectUserBill.ProjectId,
                    TenantId = AbpSession.TenantId
                };
                await _workScope.InsertAndGetIdAsync(history);
            }
            var userInfo = await _workScope.GetAll<LinkedResource>()
                .Where( lr => lr.Id == linkedId)
                .Select(lr => new GetUserInfo()
                {
                    Id = lr.UserId,
                    EmailAddress = lr.User.EmailAddress,
                    UserName = lr.User.UserName,
                    AvatarPath = lr.User.AvatarPath ?? String.Empty,
                    UserType = lr.User.UserType,
                    PositionId = lr.User.PositionId,
                    PositionColor = lr.User.Position.Color,
                    PositionName = lr.User.Position.ShortName,
                    UserLevel = lr.User.UserLevel,
                    BranchColor = lr.User.Branch.Color,
                    BranchDisplayName = lr.User.Branch.DisplayName,
                    IsActive = lr.User.IsActive,
                    FullName = lr.User.FullName,
                    Contribute = lr.Contribute
                })
                .FirstOrDefaultAsync();
            return userInfo;
        }

        public async Task UpdateLinkOneLinkedResource(LinkedResourceDto input)
        {
            var checkExist = await _workScope.GetAll<LinkedResource>()
                .Where(lr => lr.ProjectUserBillId == input.ProjectUserBillId && lr.UserId == input.UserId)
                .FirstOrDefaultAsync();
            if(await CheckTotalContribute(input.ProjectUserBillId, input.Contribute, input.UserId))
            {
                throw new UserFriendlyException("Total contribution cannot exceed 100%.");
            }
            checkExist.Contribute = input.Contribute;
            await _workScope.UpdateAsync(checkExist);
        }

        public async Task<AccountAssetDto> CreateAccountAsset(AccountAsset input)
        {
            ValidateProjectUserBill(input.ProjectUserBillId);

            var projectAsset = await _workScope.GetAsync<ProjectAsset>(input.ProjectAssetId);
            if (projectAsset == null)
                throw new UserFriendlyException("Project asset not found!");

            var entity = new AccountAsset
            {
                ProjectUserBillId = input.ProjectUserBillId,
                ProjectAssetId = input.ProjectAssetId,
            };

            var id = await _workScope.InsertAndGetIdAsync(entity);
            return await GetAccountAssetDtoAsync(id);
        }

        public async Task DeleteAccountAsset(long accountAssetId)
        {
            var accountAsset = await _workScope.GetAsync<AccountAsset>(accountAssetId);
            if (accountAsset == null)
                throw new UserFriendlyException("AccountAsset not exist !");

            await _workScope.DeleteAsync(accountAsset);
        }

        public async Task<AccountAssetDto> UpdateAccountAsset(AccountAsset input)
        {
            var accountAsset = await _workScope.GetAsync<AccountAsset>(input.Id);
            if (accountAsset == null)
            {
                throw new UserFriendlyException("AccountAsset not exist !");
            }

            if (input.ProjectUserBillId > 0)
            {
                ValidateProjectUserBill(input.ProjectUserBillId);
                accountAsset.ProjectUserBillId = input.ProjectUserBillId;
            }

            if (input.ProjectAssetId > 0)
            {
                var projectAsset = await _workScope.GetAsync<ProjectAsset>(input.ProjectAssetId);
                if (projectAsset == null)
                {
                    throw new UserFriendlyException("Project asset not found!");
                }

                accountAsset.ProjectAssetId = input.ProjectAssetId;
            }

            await _workScope.UpdateAsync(accountAsset);
            return await GetAccountAssetDtoAsync(accountAsset.Id);
        }

        private async Task<AccountAssetDto> GetAccountAssetDtoAsync(long accountAssetId)
        {
            return await _workScope.GetAll<AccountAsset>()
                .Where(x => x.Id == accountAssetId)
                .Select(x => new AccountAssetDto
                {
                    Id = x.Id,
                    ProjectUserBillId = x.ProjectUserBillId,
                    ProjectAssetId = x.ProjectAssetId,
                    ProjectAssetTypeId = x.ProjectAsset.ProjectAssetTypeId,
                    ProjectAssetTypeName = x.ProjectAsset.ProjectAssetType.Name,
                    AccountTypeId = x.ProjectAsset.AccountTypeId,
                    AccountTypeName = x.ProjectAsset.AccountType.Name,
                    AccountAssetCreatorId = x.ProjectAsset.AccountAssetCreatorId,
                    AccountAssetCreatorName = x.ProjectAsset.AccountAssetCreator.Name,
                    TypeLoginId = x.ProjectAsset.TypeLoginId,
                    TypeLoginName = x.ProjectAsset.TypeLogin.Name,
                    AssetName = x.ProjectAsset.AssetName,
                })
                .FirstOrDefaultAsync();
        }

        private async Task<bool> CheckTotalContribute(long projectUserBillId, byte contribute, long userId)
        {
            var totalContribute = await _workScope.GetAll<LinkedResource>()
                .Where(lr => lr.ProjectUserBillId == projectUserBillId && lr.UserId != userId)
                .SumAsync(lr => lr.Contribute);

            if (totalContribute + contribute > 100)
            {
                return true;
            }
            return false;
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

                var contributionHistories = await _workScope.GetAll<WeeklyContributionHistory>()
                    .Where(wch => wch.UserId == userId && wch.ProjectUserBillId == input.ProjectUserBillId && wch.PMReportId == input.PMReportId)
                    .ToListAsync();

                if (contributionHistories.Count > 0)
                {
                    foreach (var history in contributionHistories)
                    {
                        await _workScope.DeleteAsync(history);
                    }
                }

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

        public async Task<GetCvBillAccountDto> UploadCvBillAccount(UploadCvBillAccountDto input)
        {
            var projectUserBill = await _workScope.GetAsync<Entities.ProjectUserBill>(input.Id);
            var filename = DateTimeUtils.NowToyyyyMMddHHmmssfff() + "_" + input.SelectedFile.FileName.Replace(" ", "_");
            var filePath = await _uploadFileService.UploadCvAsync(input.SelectedFile, filename);
            if (string.IsNullOrEmpty(filePath))
            {
                throw new UserFriendlyException("File Upload Failed");
            }
            projectUserBill.LinkCV = filePath;
            await _workScope.UpdateAsync(projectUserBill);
            return new GetCvBillAccountDto()
            {
                Id = projectUserBill.Id,
                LinkCV = projectUserBill.LinkCV
            };
        }
    }
}
