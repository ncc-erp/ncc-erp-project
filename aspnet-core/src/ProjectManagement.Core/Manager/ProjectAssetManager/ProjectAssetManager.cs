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
                .Include(x => x.ProjectResource)
                .Where(x => x.ProjectId == projectId)
                .Select(x => new ProjectAssetDto
                {
                    Id = x.Id,
                    ProjectResourceId = x.ProjectResourceId,
                    ProjectResourceName = x.ProjectResource != null ? x.ProjectResource.Name : string.Empty,
                    AssetName = x.AssetName,
                })
                .ToListAsync();

            return assets;
        }

        public async Task<List<ProjectAssetDto>> Create(long projectId, List<ProjectAssetDto> input)
        {
            if (input == null || input.Count == 0)
                throw new UserFriendlyException("At least one project resource is required!");

            var results = new List<ProjectAssetDto>();

            foreach (var item in input)
            {
                if (item.ProjectResourceId <= 0)
                    throw new UserFriendlyException("Project resource is required!");

                var projectResource = await WorkScope.GetAsync<ProjectResource>(item.ProjectResourceId);
                if (projectResource == null)
                    throw new UserFriendlyException("Project resource not found!");

                var assetName = string.IsNullOrWhiteSpace(item.AssetName)
                    ? projectResource.Name
                    : item.AssetName.Trim();

                var projectAsset = new ProjectAsset
                {
                    ProjectId = projectId,
                    ProjectResourceId = item.ProjectResourceId,
                    AssetName = assetName
                };

                var id = await WorkScope.InsertAndGetIdAsync(projectAsset);

                results.Add(new ProjectAssetDto
                {
                    Id = id,
                    ProjectResourceId = item.ProjectResourceId,
                    ProjectResourceName = projectResource.Name,
                    AssetName = assetName,
                    
                });
            }

            return results;
        }

        public async Task UpdateUserAsset(long userId, long projectId, List<long> projectAssetIds)
        {
            if (projectAssetIds == null)
                projectAssetIds = new List<long>();

            var existingAssets = await WorkScope.All<ProjectUserAsset>()
                .Include(x => x.ProjectAsset)
                .Where(x => x.UserId == userId && x.ProjectAsset.ProjectId == projectId)
                .ToListAsync();

            foreach (var asset in existingAssets)
            {
                await WorkScope.DeleteAsync(asset);
            }

            foreach (var projectAssetId in projectAssetIds)
            {
                var projectAsset = await WorkScope.GetAsync<ProjectAsset>(projectAssetId);
                if (projectAsset == null || projectAsset.ProjectId != projectId)
                    throw new UserFriendlyException("Project asset not found in this project!");

                var userProjectAsset = new ProjectUserAsset
                {
                    UserId = userId,
                    ProjectAssetId = projectAssetId
                };

                await WorkScope.InsertAsync(userProjectAsset);
            }
        }

        public async Task<ProjectAssetDto> Edit(long projectId, ProjectAssetDto input)
        {
            var projectAsset = await WorkScope.GetAsync<ProjectAsset>(input.Id);
            if (projectAsset == null)
                throw new UserFriendlyException("Project asset not found!");

            if (projectAsset.ProjectId != projectId)
                throw new UserFriendlyException("Project asset does not belong to this project!");

            var projectResource = await WorkScope.GetAsync<ProjectResource>(input.ProjectResourceId);
            if (projectResource == null)
                throw new UserFriendlyException("Project resource not found!");

            var assetName = string.IsNullOrWhiteSpace(input.AssetName)
                ? projectResource.Name
                : input.AssetName.Trim();

            projectAsset.ProjectResourceId = input.ProjectResourceId;
            projectAsset.AssetName = assetName;
            await WorkScope.UpdateAsync(projectAsset);

            return new ProjectAssetDto
            {
                Id = projectAsset.Id,
                ProjectResourceId = projectAsset.ProjectResourceId,
                ProjectResourceName = projectResource.Name,
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
