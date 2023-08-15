using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Technologys.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Technologys
{
    [AbpAuthorize]
    public class TechnologyAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Technologies)]
        public async Task<GridResult<TechnologyDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Technology>()
                .Select(s => new TechnologyDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Color = s.Color,
                });
            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<TechnologyDto>> GetAll()
        {
            return await WorkScope.GetAll<Technology>()
                 .Select(s => new TechnologyDto
                 {
                     Id = s.Id,
                     Name = s.Name
                 }).ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Technologies_Create)]
        public async Task<TechnologyDto> Create(TechnologyDto input)
        {
            var isExist = await WorkScope.GetAll<Technology>().AnyAsync(x => x.Name == input.Name);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name <b>{0}</b> already exist !", input.Name));

            await WorkScope.InsertAsync(ObjectMapper.Map<Technology>(input));

            return input;
        }
        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Technologies_Edit)]
        public async Task<TechnologyDto> Update(TechnologyDto input)
        {
            var Technology = await WorkScope.GetAsync<Technology>(input.Id);

            var isExist = await WorkScope.GetAll<Technology>().AnyAsync(x => x.Id != input.Id && x.Name == input.Name);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name <b>{0}</b> already exist !", input.Name));

            await WorkScope.UpdateAsync(ObjectMapper.Map<TechnologyDto, Technology>(input, Technology));
            return input;
        }


        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Technologies_Delete)]
        public async Task Delete(long TechnologyId)
        {
            var hasProjectTechnology = await WorkScope.GetAll<ProjectTechnology>().AnyAsync(s => s.TechnologyId == TechnologyId);
            if (hasProjectTechnology)
                throw new UserFriendlyException(String.Format("Technology Id {0} has project", TechnologyId));
            await WorkScope.GetRepo<Technology>().DeleteAsync(TechnologyId);
        }
    }
}

