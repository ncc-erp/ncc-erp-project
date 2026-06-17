using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Manager.ProjectAssetManager;
using ProjectManagement.Manager.ProjectAssetManager.Dto;
using Abp.Authorization;
using ProjectManagement.Authorization;

namespace ProjectManagement.APIs.ProjectAssets
{
    [AbpAuthorize]
    public class ProjectAssetsAppService : ProjectManagementAppServiceBase
    {
        private readonly ProjectAssetManager _projectAssetManager;

        public ProjectAssetsAppService(ProjectAssetManager projectAssetManager)
        {
            _projectAssetManager = projectAssetManager;
        }

        [HttpGet]
        public async Task<List<GetAllProjectAssetDto>> GetAllAssetsByProjectId(long projectId)
        {
            return await _projectAssetManager.GetAllAssetsByProjectId(projectId);
        }

        [HttpPost]
        public async Task<ProjectAssetDto> Create(long projectId, ProjectAssetDto input)
        {
            if (input == null)
                throw new UserFriendlyException("Project asset is required!");

            if (input.ProjectAssetTypeId <= 0)
                throw new UserFriendlyException("Project asset type is required!");

            if (input.AccountTypeId <= 0)
                throw new UserFriendlyException("Account type is required!");

            if (input.AccountAssetCreatorId <= 0)
                throw new UserFriendlyException("Account asset creator is required!");

            if (input.TypeLoginId <= 0)
                throw new UserFriendlyException("Type login is required!");

            return await _projectAssetManager.Create(projectId, input);
        }

        [HttpPost]
        public async Task UpdateUserAsset(long userId, long projectId, List<long> projectAssetIds)
        {
            if (userId <= 0)
                throw new UserFriendlyException("User id is invalid!");

            if (projectId <= 0)
                throw new UserFriendlyException("Project id is invalid!");

            if (projectAssetIds == null)
                projectAssetIds = new List<long>();

            await _projectAssetManager.UpdateUserAsset(userId, projectId, projectAssetIds);
        }

        [HttpPut]
        public async Task<ProjectAssetDto> Edit(long projectId, ProjectAssetDto input)
        {
            if (input.Id <= 0)
                throw new UserFriendlyException("Project asset id is invalid!");

            if (input.ProjectAssetTypeId <= 0)
                throw new UserFriendlyException("Project asset type is required!");

            if (input.AccountTypeId <= 0)
                throw new UserFriendlyException("Account type is required!");

            if (input.AccountAssetCreatorId <= 0)
                throw new UserFriendlyException("Account asset creator is required!");

            if (input.TypeLoginId <= 0)
                throw new UserFriendlyException("Type login is required!");

            return await _projectAssetManager.Edit(projectId, input);
        }

        [HttpDelete]
        public async Task Delete(long projectAssetId)
        {
            if (projectAssetId <= 0)
                throw new UserFriendlyException("Project asset id is invalid!");

            await _projectAssetManager.Delete(projectAssetId);
        }

        [HttpGet]
        public async Task<List<ProjectAssetDropdownDto>> GetAllForDropdown(long projectId)
        {
            return await _projectAssetManager.GetAllForDropdown(projectId);
        }
    }
}
