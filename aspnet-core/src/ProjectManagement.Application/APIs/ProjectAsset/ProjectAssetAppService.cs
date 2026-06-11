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
    [AbpAuthorize]
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
        public async Task<List<ProjectAssetDto>> Create(long projectId, List<ProjectAssetDto> input)
        {
            if (input == null || input.Count == 0)
                throw new UserFriendlyException("At least one project resource is required!");

            foreach (var item in input)
            {
                if (item.ProjectResourceId <= 0)
                    throw new UserFriendlyException("Project resource is required!");
            }

            return await _projectAssetManager.Create(projectId, input);
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

            if (input.ProjectResourceId <= 0)
                throw new UserFriendlyException("Project resource is required!");

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
