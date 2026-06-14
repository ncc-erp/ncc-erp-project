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
        public async Task<List<ProjectAssetDto>> GetAllAssetsByProjectId(long projectId)
        {
            var assets = await WorkScope.All<ProjectAsset>()
                .Include(x => x.ProjectAssetType)
                .Where(x => x.ProjectId == projectId)
                .Select(x => new ProjectAssetDto
                {
                    Id = x.Id,
                    ProjectAssetTypeId = x.ProjectAssetTypeId,
                    ProjectAssetTypeName = x.ProjectAssetType != null ? x.ProjectAssetType.Name : string.Empty,
                    AssetName = x.AssetName,
                })
                .ToListAsync();

            return assets;
        }

        public async Task<List<ProjectAssetDto>> Create(long projectId, List<ProjectAssetDto> input)
        {
            if (input == null || input.Count == 0)
                throw new UserFriendlyException("At least one project asset is required!");

            var projectAssetTypeIds = input
                .Select(x => x.ProjectAssetTypeId)
                .Where(x => x > 0)
                .Distinct()
                .ToList();

            var projectAssetTypes = await WorkScope.GetAll<ProjectAssetType>()
                .Where(x => projectAssetTypeIds.Contains(x.Id))
                .ToDictionaryAsync(x => x.Id);

            foreach (var item in input)
            {
                if (item.ProjectAssetTypeId <= 0)
                    throw new UserFriendlyException("Project asset type is required!");

                if (!projectAssetTypes.ContainsKey(item.ProjectAssetTypeId))
                    throw new UserFriendlyException("Project asset type not found!");
            }

            var projectAssetsToInsert = input.Select(item =>
            {
                var projectAssetType = projectAssetTypes[item.ProjectAssetTypeId];
                var assetName = string.IsNullOrWhiteSpace(item.AssetName)
                    ? projectAssetType.Name
                    : item.AssetName.Trim();

                return new ProjectAsset
                {
                    ProjectId = projectId,
                    ProjectAssetTypeId = item.ProjectAssetTypeId,
                    AssetName = assetName
                };
            }).ToList();

            var insertedAssets = await WorkScope.InsertRangeAsync(projectAssetsToInsert);

            return insertedAssets.Select(x => new ProjectAssetDto
            {
                Id = x.Id,
                ProjectAssetTypeId = x.ProjectAssetTypeId,
                ProjectAssetTypeName = projectAssetTypes[x.ProjectAssetTypeId].Name,
                AssetName = x.AssetName,
            }).ToList();
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

            var assetName = string.IsNullOrWhiteSpace(input.AssetName)
                ? projectAssetType.Name
                : input.AssetName.Trim();

            projectAsset.ProjectAssetTypeId = input.ProjectAssetTypeId;
            projectAsset.AssetName = assetName;
            await WorkScope.UpdateAsync(projectAsset);

            return new ProjectAssetDto
            {
                Id = projectAsset.Id,
                ProjectAssetTypeId = projectAsset.ProjectAssetTypeId,
                ProjectAssetTypeName = projectAssetType.Name,
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
