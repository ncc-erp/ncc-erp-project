using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Tags.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Tags
{
    public class TagAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        public async Task<GridResult<TagDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Tag>()
                .Select(x => new TagDto
                {
                    Id = x.Id,
                    Name = x.Name,
                });
            return await query.GetGridResult(query, input);
        }
        [HttpGet]
        public async Task<List<TagDto>> GetAll()
        {
            var query = WorkScope.GetAll<Tag>()
                .Select(x => new TagDto
                {
                    Id = x.Id,
                    Name = x.Name,
                });
            return await query.ToListAsync();
        }
        [HttpPost]
        public async Task<TagDto> Create(TagDto input)
        {
            var isExist = await WorkScope.GetAll<Tag>().AnyAsync(x => x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException(String.Format("Name already exist"));

            await WorkScope.InsertAndGetIdAsync<Tag>(ObjectMapper.Map<Tag>(input));
            return input;
        }
        [HttpPut]
        public async Task<TagDto> Update(TagDto input)
        {
            var tag = await WorkScope.GetAsync<Tag>(input.Id);

            var isExist = await WorkScope.GetAll<Tag>().AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException(String.Format("Name already exist"));

            await WorkScope.UpdateAsync(ObjectMapper.Map<TagDto, Tag>(input, tag));
            return input;
        }
        [HttpDelete]
        public async Task Delete(long tagId)
        {
            var isCPUserResultTagExist = await WorkScope.GetAll<CheckPointUserResultTag>().AnyAsync(x => x.TagId == tagId);
            if (isCPUserResultTagExist)
                throw new UserFriendlyException(String.Format("Tag này đã được sử dụng"));

            await WorkScope.DeleteAsync<Tag>(tagId);
        }
    }
}
