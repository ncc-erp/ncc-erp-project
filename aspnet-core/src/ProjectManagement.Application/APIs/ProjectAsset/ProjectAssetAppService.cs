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

namespace ProjectManagement.APIs.ProjectAsset
{
    public class ProjectAssetAppService : ProjectManagementAppServiceBase
    {
        private readonly ProjectAssetManager _projectAssetManager;

        public ProjectAssetAppService(ProjectAssetManager projectAssetManager)
        {
            _projectAssetManager = projectAssetManager;
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<ProjectAssetDto>> GetAllAssetsByProjectId(long projectId)
        {
            return await _projectAssetManager.GetAllAssetsByProjectId(projectId);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<ProjectAssetDto> Create(long projectId, ProjectAssetDto input)
        {
            if (string.IsNullOrWhiteSpace(input.AssetName))
                throw new UserFriendlyException("Asset name is required!");

            return await _projectAssetManager.Create(projectId, input);
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<GetAllUserProjectAsset>> GetAllUserProjectAssetByProjectId(long projectId, bool viewHistory = false)
        {
            return await _projectAssetManager.GetAllUserProjectAssetByProjectId(projectId, viewHistory);
        }

        [HttpPost]
        [AbpAuthorize]
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
        [AbpAuthorize]
        public async Task<ProjectAssetDto> Edit(long projectId, ProjectAssetDto input)
        {
            if (input.Id <= 0)
                throw new UserFriendlyException("Project asset id is invalid!");

            if (string.IsNullOrWhiteSpace(input.AssetName))
                throw new UserFriendlyException("Asset name is required!");

            return await _projectAssetManager.Edit(projectId, input);
        }

        [HttpDelete]
        [AbpAuthorize]
        public async Task Delete(long projectAssetId)
        {
            if (projectAssetId <= 0)
                throw new UserFriendlyException("Project asset id is invalid!");

            await _projectAssetManager.Delete(projectAssetId);
        }
    }
}
