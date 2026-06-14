using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.AccountTypes.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.AccountTypes
{
    [AbpAuthorize]
    public class AccountTypeAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_AccountTypes)]
        public async Task<GridResult<AccountTypeDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<AccountType>()
                .Select(x => new AccountTypeDto
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<AccountTypeDto>> GetAll()
        {
            return await WorkScope.GetAll<AccountType>()
                .Select(x => new AccountTypeDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_AccountTypes_Create)]
        public async Task<AccountTypeDto> Create(AccountTypeDto input)
        {
            var isExist = await WorkScope.GetAll<AccountType>().AnyAsync(x => x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("AccountType already exist !");

            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<AccountType>(input));
            return input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_AccountTypes_Edit)]
        public async Task<AccountTypeDto> Update(AccountTypeDto input)
        {
            var accountType = await WorkScope.GetAsync<AccountType>(input.Id);
            if (accountType == null)
                throw new UserFriendlyException("AccountType not exist !");

            var isExist = await WorkScope.GetAll<AccountType>()
                .AnyAsync(x => x.Id != input.Id && x.Name == input.Name);
            if (isExist)
                throw new UserFriendlyException("AccountType already exist !");

            await WorkScope.UpdateAsync(ObjectMapper.Map<AccountTypeDto, AccountType>(input, accountType));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_AccountTypes_Delete)]
        public async Task Delete(long accountTypeId)
        {
            var accountType = await WorkScope.GetAsync<AccountType>(accountTypeId);
            if (accountType == null)
                throw new UserFriendlyException("AccountType not exist !");

            await WorkScope.DeleteAsync(accountType);
        }
    }
}
