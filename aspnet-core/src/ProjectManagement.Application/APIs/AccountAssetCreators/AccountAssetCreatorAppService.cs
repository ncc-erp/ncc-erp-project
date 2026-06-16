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
    public class AccountAssetCreatorAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_AccountAssetCreators)]
        public async Task<GridResult<AccountAssetCreatorDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<AccountAssetCreator>()
                .Select(x => new AccountAssetCreatorDto
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<AccountAssetCreatorDto>> GetAll()
        {
            return await WorkScope.GetAll<AccountAssetCreator>()
                .Select(x => new AccountAssetCreatorDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_AccountAssetCreators_Create)]
        public async Task<AccountAssetCreatorDto> Create(AccountAssetCreatorDto input)
        {
            var isExist = await WorkScope.GetAll<AccountAssetCreator>().AnyAsync(x => x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("Creator already exist !");

            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<AccountAssetCreator>(input));
            return input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_AccountAssetCreators_Edit)]
        public async Task<AccountAssetCreatorDto> Update(AccountAssetCreatorDto input)
        {
            var creator = await WorkScope.GetAsync<AccountAssetCreator>(input.Id);
            if (creator == null)
                throw new UserFriendlyException("Creator not exist !");

            var isExist = await WorkScope.GetAll<AccountAssetCreator>()
                .AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("Creator already exist !");

            await WorkScope.UpdateAsync(ObjectMapper.Map<AccountAssetCreatorDto, AccountAssetCreator>(input, creator));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_AccountAssetCreators_Delete)]
        public async Task Delete(long creatorId)
        {
            var creator = await WorkScope.GetAsync<AccountAssetCreator>(creatorId);
            if (creator == null)
                throw new UserFriendlyException("Creator not exist !");

            var hasProjectAsset = await WorkScope.GetAll<ProjectAsset>().AnyAsync(x => x.AccountAssetCreatorId == creatorId);
            if (hasProjectAsset)
                throw new UserFriendlyException("There is ProjectAsset with this Creator");

            await WorkScope.DeleteAsync(creator);
        }
    }
}
