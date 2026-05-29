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
                .Where(x => x.ProjectId == projectId)
                .Select(x => new ProjectAssetDto
                {
                    Id = x.Id,
                    AssetName = x.AssetName
                })
                .ToListAsync();

            return assets;
        }

        public async Task<ProjectAssetDto> Create(long projectId, ProjectAssetDto input)
        {
            var isExist = await WorkScope.All<ProjectAsset>()
                .AnyAsync(x => x.ProjectId == projectId && x.AssetName == input.AssetName);
            
            if (isExist)
                throw new UserFriendlyException("Asset already exists in this project!");

            var projectAsset = new ProjectAsset
            {
                ProjectId = projectId,
                AssetName = input.AssetName
            };

            var id = await WorkScope.InsertAndGetIdAsync(projectAsset);
            
            return new ProjectAssetDto
            {
                Id = id,
                AssetName = input.AssetName
            };
        }

        public async Task<List<GetAllUserProjectAsset>> GetAllUserProjectAssetByProjectId(long projectId, bool viewHistory = false)
        {
            var query = WorkScope.All<ProjectUser>()
                .Where(x => x.ProjectId == projectId)
                .Where(x => x.User.UserType != UserType.FakeUser);

            if (!viewHistory)
            {
                query = query.Where(x => x.Status == ProjectUserStatus.Present && x.AllocatePercentage > 0);
            }

            var projectUsers = await query
                .Include(x => x.User)
                .ToListAsync();

            var result = new List<GetAllUserProjectAsset>();

            foreach (var projectUser in projectUsers)
            {
                var userProjectAssets = await WorkScope.All<ProjectUserAsset>()
                    .Where(x => x.UserId == projectUser.UserId)
                    .Include(x => x.ProjectAsset)
                    .Include(x => x.User)
                    .ToListAsync();

                var dto = new GetAllUserProjectAsset
                {
                    Id = projectUser.UserId,
                    UserId = projectUser.UserId,
                    EmailAddress = projectUser.User.EmailAddress,
                    AvatarPath = projectUser.User.AvatarPath,
                    UserType = projectUser.User.UserType,
                    Branch = projectUser.User.BranchOld,
                    BranchColor = projectUser.User.Branch != null ? projectUser.User.Branch.Color : null,
                    BranchDisplayName = projectUser.User.Branch != null ? projectUser.User.Branch.DisplayName : null,
                    PositionId = projectUser.User.PositionId,
                    PositionColor = projectUser.User.Position != null ? projectUser.User.Position.Color : null,
                    PositionName = projectUser.User.Position != null ? projectUser.User.Position.Name : null,
                    FullName = projectUser.User.FullName,
                    UserLevel = projectUser.User.UserLevel,
                    ProjectAssets = userProjectAssets
                        .Select(x => x.ProjectAsset.AssetName)
                        .ToList()
                };

                result.Add(dto);
            }

            return result;
        }

        public async Task UpdateUserAsset(long userId, List<long> projectAssetIds)
        {
            if (projectAssetIds == null)
                projectAssetIds = new List<long>();

            var existingAssets = await WorkScope.All<ProjectUserAsset>()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            foreach (var asset in existingAssets)
            {
                await WorkScope.DeleteAsync(asset);
            }

            foreach (var projectAssetId in projectAssetIds)
            {
                var projectAsset = await WorkScope.GetAsync<ProjectAsset>(projectAssetId);
                if (projectAsset == null)
                    throw new UserFriendlyException("Project asset not found!");

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

            var isExist = await WorkScope.All<ProjectAsset>()
                .AnyAsync(x => x.ProjectId == projectId && x.AssetName == input.AssetName && x.Id != input.Id);

            if (isExist)
                throw new UserFriendlyException("Asset name already exists in this project!");

            projectAsset.AssetName = input.AssetName;
            await WorkScope.UpdateAsync(projectAsset);

            return new ProjectAssetDto
            {
                Id = projectAsset.Id,
                AssetName = projectAsset.AssetName
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
