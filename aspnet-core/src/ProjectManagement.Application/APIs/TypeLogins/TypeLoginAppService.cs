using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Skills.Dto;
using ProjectManagement.APIs.TypeLogins.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.TypeLogins
{
    [AbpAuthorize]
    public class TypeLoginAppService :ProjectManagementAppServiceBase
    {
        [HttpPost]
        public async Task<GridResult<TypeLoginDto>> GetAllPagging(GridParam input)
        {
            var query = WorkScope.GetAll<TypeLogin>().Select(x => new TypeLoginDto
            {
                Id = x.Id,
                Name = x.Name,
            });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<TypeLoginDto>> GetAll()
        {
            var query = WorkScope.GetAll<TypeLogin>().Select(x => new TypeLoginDto
            {
                Id = x.Id,
                Name = x.Name,
            }).OrderBy(x => x.Name);

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<TypeLoginDto> Create(TypeLoginDto input)
        {
            var isExist = await WorkScope.GetAll<TypeLogin>().AnyAsync(x => x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("TypeLogin already exist");

            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<TypeLogin>(input));

            return input;
        }

        [HttpPut]
        public async Task<TypeLoginDto> Update(TypeLoginDto input)
        {
            var typeLogin = await WorkScope.GetAsync<TypeLogin>(input.Id);
            if (typeLogin == null)
                throw new UserFriendlyException("TypeLogin not exist");

            var isExist = await WorkScope.GetAll<TypeLogin>().AnyAsync(x => x.Name == input.Name && x.Id != input.Id);
            if (isExist)
                throw new UserFriendlyException("TypeLogin already exist");

            await WorkScope.UpdateAsync(ObjectMapper.Map<TypeLoginDto, TypeLogin>(input, typeLogin));
            return input;
        }

        [HttpDelete]
        public async Task Delete(long typeLoginId)
        {
            var typeLogin = await WorkScope.GetAsync<TypeLogin>(typeLoginId);
            if (typeLogin == null)
                throw new UserFriendlyException("TypeLogin not exist");
            var hasProjectAsset = await WorkScope.GetAll<ProjectAsset>().AnyAsync(x => x.TypeLoginId == typeLoginId);
            if (hasProjectAsset)
                throw new UserFriendlyException("There is ProjecAsset with this TypeLogin");

            await WorkScope.DeleteAsync(typeLogin);
        }
    }
}
