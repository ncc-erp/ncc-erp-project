using Microsoft.EntityFrameworkCore;
using NccCore.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Manager.ProjectAssetManager.Dto;
using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using Abp.UI;

namespace ProjectManagement.Manager.ProjectAssetManager
{
    public class ProjectAssetManager : BaseManager
    {
        public ProjectAssetManager(IWorkScope workScope) : base(workScope)
        {
        }
        public async Task<List<GetAllProjectAssetDto>> GetAllAssetsByProjectId(long projectId)
        {
            var assets = await WorkScope.All<ProjectAsset>()
                .Include(x => x.ProjectAssetType)
                .Include(x => x.AccountType)
                .Include(x => x.AccountAssetCreator)
                .Include(x => x.TypeLogin)
                .Where(x => x.ProjectId == projectId)
                .Select(x => new GetAllProjectAssetDto
                {
                    Id = x.Id,
                    ProjectAssetTypeId = x.ProjectAssetTypeId,
                    ProjectAssetTypeName = x.ProjectAssetType != null ? x.ProjectAssetType.Name : string.Empty,
                    AccountTypeId = x.AccountTypeId,
                    AccountTypeName = x.AccountType != null ? x.AccountType.Name : string.Empty,
                    AccountAssetCreatorId = x.AccountAssetCreatorId,
                    AccountAssetCreatorName = x.AccountAssetCreator != null ? x.AccountAssetCreator.Name : string.Empty,
                    TypeLoginId = x.TypeLoginId,
                    TypeLoginName = x.TypeLogin != null ? x.TypeLogin.Name : string.Empty,
                    AssetName = x.AssetName,
                    ProjectUsers = x.ProjectUserAssets
                        .Select(pua => new GetAllUserProjectAssetDto
                        {
                            Id = pua.Id,
                            UserId = pua.UserId,
                            EmailAddress = pua.User.EmailAddress,
                            AvatarPath = pua.User.AvatarPath,
                            UserType = pua.User.UserType,
                            Branch = pua.User.BranchOld,
                            FullName = pua.User.FullName,
                            BranchColor = pua.User.Branch.Color,
                            BranchDisplayName = pua.User.Branch.DisplayName,
                            PositionId = pua.User.PositionId,
                            PositionColor = pua.User.Position.Color,
                            PositionName = pua.User.Position.ShortName,
                            UserLevel = pua.User.UserLevel
                        }).ToList(),
                    BillAccounts = x.AccountAssets
                        .Select(ar => new GetAllUserProjectAssetDto
                        {
                            Id = ar.Id,
                            UserId = ar.ProjectUserBill.UserId,
                            EmailAddress = ar.ProjectUserBill.User.EmailAddress,
                            AvatarPath = ar.ProjectUserBill.User.AvatarPath,
                            UserType = ar.ProjectUserBill.User.UserType,
                            Branch = ar.ProjectUserBill.User.BranchOld,
                            FullName = ar.ProjectUserBill.User.FullName,
                            BranchColor = ar.ProjectUserBill.User.Branch.Color,
                            BranchDisplayName = ar.ProjectUserBill.User.Branch.DisplayName,
                            PositionId = ar.ProjectUserBill.User.PositionId,
                            PositionColor = ar.ProjectUserBill.User.Position.Color,
                            PositionName = ar.ProjectUserBill.User.Position.ShortName,
                            UserLevel = ar.ProjectUserBill.User.UserLevel
                        }).ToList()
                })
                .ToListAsync();

            return assets;
        }

        public async Task<List<ProjectAssetDropdownDto>> GetAllForDropdown(long projectId)
        {
            return await WorkScope.All<ProjectAsset>()
                .Where(x => x.ProjectId == projectId)
                .Select(x => new ProjectAssetDropdownDto
                {
                    Id = x.Id,
                    AssetName = x.AssetName
                })
                .ToListAsync();
        }

        public async Task<ProjectAssetDto> Create(long projectId, ProjectAssetDto input)
        {
            var projectAssetType = await WorkScope.GetAsync<ProjectAssetType>(input.ProjectAssetTypeId);
            if (projectAssetType == null)
                throw new UserFriendlyException("Project asset type not found!");

            var accountType = await WorkScope.GetAsync<AccountType>(input.AccountTypeId);
            if (accountType == null)
                throw new UserFriendlyException("Account type not found!");

            var accountAssetCreator = await WorkScope.GetAsync<AccountAssetCreator>(input.AccountAssetCreatorId);
            if (accountAssetCreator == null)
                throw new UserFriendlyException("Account asset creator not found!");

            var typeLogin = await WorkScope.GetAsync<TypeLogin>(input.TypeLoginId);
            if (typeLogin == null)
                throw new UserFriendlyException("Type login not found!");

            var assetName = string.IsNullOrWhiteSpace(input.AssetName)
                ? projectAssetType.Name
                : input.AssetName.Trim();

            var projectAsset = new ProjectAsset
            {
                ProjectId = projectId,
                ProjectAssetTypeId = input.ProjectAssetTypeId,
                AccountTypeId = input.AccountTypeId,
                AccountAssetCreatorId = input.AccountAssetCreatorId,
                TypeLoginId = input.TypeLoginId,
                AssetName = assetName
            };

            var insertedAsset = await WorkScope.InsertAsync(projectAsset);

            return new ProjectAssetDto
            {
                Id = insertedAsset.Id,
                ProjectAssetTypeId = insertedAsset.ProjectAssetTypeId,
                ProjectAssetTypeName = projectAssetType.Name,
                AccountTypeId = insertedAsset.AccountTypeId,
                AccountTypeName = accountType.Name,
                AccountAssetCreatorId = insertedAsset.AccountAssetCreatorId,
                AccountAssetCreatorName = accountAssetCreator.Name,
                TypeLoginId = insertedAsset.TypeLoginId,
                TypeLoginName = typeLogin.Name,
                AssetName = insertedAsset.AssetName,
            };
        }

        public async Task UpdateUserAsset(long userId, long projectId, List<long> projectAssetIds)
        {
            if (projectAssetIds == null)
                projectAssetIds = new List<long>();

            var distinctProjectAssetIds = projectAssetIds
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            var existingAssets = await WorkScope.All<ProjectUserAsset>()
                .Include(x => x.ProjectAsset)
                .Where(x => x.UserId == userId && x.ProjectAsset.ProjectId == projectId)
                .ToListAsync();

            foreach (var asset in existingAssets)
            {
                await WorkScope.DeleteAsync(asset);
            }

            var validProjectAssets = await WorkScope.All<ProjectAsset>()
                .Where(x => x.ProjectId == projectId && distinctProjectAssetIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            foreach (var projectAssetId in distinctProjectAssetIds)
            {
                if (!validProjectAssets.ContainsKey(projectAssetId))
                    throw new UserFriendlyException("Project asset not found in this project!");
            }

            var userProjectAssets = distinctProjectAssetIds.Select(projectAssetId => new ProjectUserAsset
            {
                UserId = userId,
                ProjectAssetId = projectAssetId
            }).ToList();

            if (userProjectAssets.Count > 0)
            {
                await WorkScope.InsertRangeAsync(userProjectAssets);
            }
        }

        public async Task<ProjectAssetDto> Edit(long projectId, ProjectAssetDto input)
        {
            var projectAsset = await WorkScope.GetAsync<ProjectAsset>(input.Id);
            if (projectAsset == null)
                throw new UserFriendlyException("Project asset not found!");

            if (projectAsset.ProjectId != projectId)
                throw new UserFriendlyException("Project asset does not belong to this project!");

            var projectAssetType = await WorkScope.GetAsync<ProjectAssetType>(input.ProjectAssetTypeId);
            if (projectAssetType == null)
                throw new UserFriendlyException("Project asset type not found!");

            var accountType = await WorkScope.GetAsync<AccountType>(input.AccountTypeId);
            if (accountType == null)
                throw new UserFriendlyException("Account type not found!");

            var accountAssetCreator = await WorkScope.GetAsync<AccountAssetCreator>(input.AccountAssetCreatorId);
            if (accountAssetCreator == null)
                throw new UserFriendlyException("Account asset creator not found!");

            var typeLogin = await WorkScope.GetAsync<TypeLogin>(input.TypeLoginId);
            if (typeLogin == null)
                throw new UserFriendlyException("Type login not found!");

            var assetName = string.IsNullOrWhiteSpace(input.AssetName)
                ? projectAssetType.Name
                : input.AssetName.Trim();

            projectAsset.ProjectAssetTypeId = input.ProjectAssetTypeId;
            projectAsset.AccountTypeId = input.AccountTypeId;
            projectAsset.AccountAssetCreatorId = input.AccountAssetCreatorId;
            projectAsset.TypeLoginId = input.TypeLoginId;
            projectAsset.AssetName = assetName;
            await WorkScope.UpdateAsync(projectAsset);

            return new ProjectAssetDto
            {
                Id = projectAsset.Id,
                ProjectAssetTypeId = projectAsset.ProjectAssetTypeId,
                ProjectAssetTypeName = projectAssetType.Name,
                AccountTypeId = projectAsset.AccountTypeId,
                AccountTypeName = accountType.Name,
                AccountAssetCreatorId = projectAsset.AccountAssetCreatorId,
                AccountAssetCreatorName = accountAssetCreator.Name,
                TypeLoginId = projectAsset.TypeLoginId,
                TypeLoginName = typeLogin.Name,
                AssetName = projectAsset.AssetName,
            };
        }

        public async Task Delete(long projectAssetId)
        {
            var projectAsset = await WorkScope.GetAsync<ProjectAsset>(projectAssetId);
            if (projectAsset == null)
                throw new UserFriendlyException("Project asset not found!");

            var hasUserAssigned = await WorkScope.All<ProjectUserAsset>()
                .AnyAsync(x => x.ProjectAssetId == projectAssetId);

            if (hasUserAssigned)
                throw new UserFriendlyException("Cannot delete project asset because there are users assigned to it!");

            await WorkScope.DeleteAsync(projectAsset);
        }
    }
}
