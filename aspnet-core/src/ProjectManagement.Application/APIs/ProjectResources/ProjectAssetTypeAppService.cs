using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.ProjectResources.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectResources
{
    [AbpAuthorize]
    public class ProjectResourceAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize]
        public async Task<List<ProjectResourceDto>> GetAll(string name = null)
        {
            var resources = await WorkScope.GetAll<ProjectResource>()
                .OrderBy(x => x.Name)
                .Select(x => new ProjectResourceDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Note = x.Note,
                    ParentId = x.ParentId
                })
                .ToListAsync();

            var childrenMap = resources
                .Where(x => x.ParentId.HasValue)
                .GroupBy(x => x.ParentId.Value)
                .ToDictionary(x => x.Key, x => x.ToList());

            if (!string.IsNullOrWhiteSpace(name))
            {
                var normalizedName = name.Trim();
                var visibleIds = resources
                    .Where(x => x.Name != null && x.Name.Contains(normalizedName, StringComparison.OrdinalIgnoreCase))
                    .Select(x => x.Id)
                    .ToHashSet();

                foreach (var item in resources.Where(x => x.ParentId.HasValue && visibleIds.Contains(x.Id)).ToList())
                {
                    var currentParentId = item.ParentId;
                    while (currentParentId.HasValue)
                    {
                        visibleIds.Add(currentParentId.Value);
                        currentParentId = resources.FirstOrDefault(x => x.Id == currentParentId.Value)?.ParentId;
                    }
                }

                return resources
                    .Where(x => !x.ParentId.HasValue && visibleIds.Contains(x.Id))
                    .Select(x => BuildTree(x, childrenMap, visibleIds))
                    .ToList();
            }

            return resources
                .Where(x => !x.ParentId.HasValue)
                .Select(x => BuildTree(x, childrenMap, null))
                .ToList();
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<ProjectResourceDto> Create(ProjectResourceDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new UserFriendlyException("Name is required!");

            var exists = await WorkScope.GetAll<ProjectResource>().AnyAsync(x => x.Name == input.Name && x.ParentId == input.ParentId);
            if (exists)
                throw new UserFriendlyException("Project resource already exists!");

            var entity = ObjectMapper.Map<ProjectResource>(input);
            input.Id = await WorkScope.InsertAndGetIdAsync(entity);
            return input;
        }

        [HttpPut]
        [AbpAuthorize]
        public async Task<ProjectResourceDto> Update(ProjectResourceDto input)
        {
            if (string.IsNullOrWhiteSpace(input.Name))
                throw new UserFriendlyException("Name is required!");

            var entity = await WorkScope.GetAsync<ProjectResource>(input.Id);
            if (entity == null)
                throw new UserFriendlyException("Project resource not found!");

            var exists = await WorkScope.GetAll<ProjectResource>()
                .AnyAsync(x => x.Id != input.Id && x.Name == input.Name && x.ParentId == input.ParentId);
            if (exists)
                throw new UserFriendlyException("Project resource already exists!");

            await WorkScope.UpdateAsync(ObjectMapper.Map(input, entity));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize]
        public async Task Delete(long id)
        {
            var entity = await WorkScope.GetAsync<ProjectResource>(id);
            if (entity == null)
                throw new UserFriendlyException("Project resource not found!");

            var hasChildren = await WorkScope.GetAll<ProjectResource>().AnyAsync(x => x.ParentId == id);
            if (hasChildren)
                throw new UserFriendlyException("Cannot delete a project resource that has child nodes!");

            await WorkScope.DeleteAsync(entity);
        }

        private static ProjectResourceDto BuildTree(
            ProjectResourceDto item,
            IReadOnlyDictionary<long, List<ProjectResourceDto>> childrenMap,
            ISet<long> visibleIds)
        {
            var dto = new ProjectResourceDto
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
