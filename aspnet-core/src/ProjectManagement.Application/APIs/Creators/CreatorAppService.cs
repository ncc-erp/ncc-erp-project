using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Creators.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Creators
{
    [AbpAuthorize]
    public class CreatorAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Creators)]
        public async Task<GridResult<CreatorDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Creator>()
                .Select(x => new CreatorDto
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<CreatorDto>> GetAll()
        {
            return await WorkScope.GetAll<Creator>()
                .Select(x => new CreatorDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Creators_Create)]
        public async Task<CreatorDto> Create(CreatorDto input)
        {
            var isExist = await WorkScope.GetAll<Creator>().AnyAsync(x => x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("Creator already exist !");

            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<Creator>(input));
            return input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Creators_Edit)]
        public async Task<CreatorDto> Update(CreatorDto input)
        {
            var creator = await WorkScope.GetAsync<Creator>(input.Id);
            if (creator == null)
                throw new UserFriendlyException("Creator not exist !");

            var isExist = await WorkScope.GetAll<Creator>()
                .AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("Creator already exist !");

            await WorkScope.UpdateAsync(ObjectMapper.Map<CreatorDto, Creator>(input, creator));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Creators_Delete)]
        public async Task Delete(long creatorId)
        {
            var creator = await WorkScope.GetAsync<Creator>(creatorId);
            if (creator == null)
                throw new UserFriendlyException("Creator not exist !");

            await WorkScope.DeleteAsync(creator);
        }
    }
}
