using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.ProjectResources.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectResources
{
    [AbpAuthorize]
    public class ProjectAssetTypeAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        public async Task<List<ProjectAssetTypeDto>> GetAll(string name = null)
        {
            var assetTypes = await WorkScope.GetAll<ProjectAssetType>()
                .OrderBy(x => x.Name)
                .Select(x => new ProjectAssetTypeDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Note = x.Note,
                    ParentId = x.ParentId
                })
                .ToListAsync();

            var childrenMap = assetTypes
                .Where(x => x.ParentId.HasValue)
                .GroupBy(x => x.ParentId.Value)
                .ToDictionary(x => x.Key, x => x.ToList());

            if (!string.IsNullOrWhiteSpace(name))
            {
                var normalizedName = name.Trim();
                var visibleIds = assetTypes
                    .Where(x => x.Name != null && x.Name.Contains(normalizedName, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Id)
                    .ToHashSet();

                foreach (var item in assetTypes.Where(x => x.ParentId.HasValue && visibleIds.Contains(x.Id)).ToList())
                {
                    var currentParentId = item.ParentId;
                    while (currentParentId.HasValue)
                    {
                        visibleIds.Add(currentParentId.Value);
                        currentParentId = assetTypes.FirstOrDefault(x => x.Id == currentParentId.Value)?.ParentId;
                    }
                }

                return assetTypes
                    .Where(x => !x.ParentId.HasValue && visibleIds.Contains(x.Id))
                    .Select(x => BuildTree(x, childrenMap, visibleIds))
                    .ToList();
            }

            return assetTypes
                .Where(x => !x.ParentId.HasValue)
                .Select(x => BuildTree(x, childrenMap, null))
                .ToList();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_ProjectAssetTypes_Create)]
        public async Task<ProjectAssetTypeDto> Create(ProjectAssetTypeDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new UserFriendlyException("Name is required!");

            var exists = await WorkScope.GetAll<ProjectAssetType>().AnyAsync(x => x.Name == input.Name && x.ParentId == input.ParentId);
            if (exists)
                throw new UserFriendlyException("Project asset type already exists!");

            var entity = ObjectMapper.Map<ProjectAssetType>(input);
            input.Id = await WorkScope.InsertAndGetIdAsync(entity);
            return input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_ProjectAssetTypes_Edit)]
        public async Task<ProjectAssetTypeDto> Update(ProjectAssetTypeDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new UserFriendlyException("Name is required!");

            var entity = await WorkScope.GetAsync<ProjectAssetType>(input.Id);
            if (entity == null)
                throw new UserFriendlyException("Project asset type not found!");

            var exists = await WorkScope.GetAll<ProjectAssetType>()
                .AnyAsync(x => x.Id != input.Id && x.Name == input.Name && x.ParentId == input.ParentId);
            if (exists)
                throw new UserFriendlyException("Project asset type already exists!");

            await WorkScope.UpdateAsync(ObjectMapper.Map(input, entity));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_ProjectAssetTypes_Delete)]
        public async Task Delete(long id)
        {
            var entity = await WorkScope.GetAsync<ProjectAssetType>(id);
            if (entity == null)
                throw new UserFriendlyException("Project asset type not found!");

            var hasChildren = await WorkScope.GetAll<ProjectAssetType>().AnyAsync(x => x.ParentId == id);
            if (hasChildren)
                throw new UserFriendlyException("Cannot delete a project asset type that has child nodes!");

            await WorkScope.DeleteAsync(entity);
        }

        private static ProjectAssetTypeDto BuildTree(
            ProjectAssetTypeDto item,
            IReadOnlyDictionary<long, List<ProjectAssetTypeDto>> childrenMap,
            ISet<long> visibleIds)
        {
            var dto = new ProjectAssetTypeDto
            {
                Id = item.Id,
                Name = item.Name,
                Note = item.Note,
                ParentId = item.ParentId
            };

            if (childrenMap.TryGetValue(item.Id, out var children))
            {
                var filteredChildren = visibleIds == null
                    ? children
                    : children.Where(x => visibleIds.Contains(x.Id)).ToList();

                dto.Childrens = filteredChildren
                    .Select(x => BuildTree(x, childrenMap, visibleIds))
                    .ToList();
            }

            return dto;
        }
    }
}
