using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Currencies.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Currencies
{
    [AbpAuthorize]
    public class CurrencyAppService : ProjectManagementAppServiceBase
    {
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Currencies)]
        public async Task<GridResult<CurrencyDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Currency>()
                .Select(x => new CurrencyDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    InvoicePaymentInfo = x.InvoicePaymentInfo
                });
            return await query.GetGridResult(query, input);
        }
        [HttpGet]
        public async Task<List<CurrencyDto>> GetAll()
        {
            var query = WorkScope.GetAll<Currency>()
              .Select(s => new CurrencyDto
              {
                  Id = s.Id,
                  Name = s.Name,
                  Code = s.Code,
                  InvoicePaymentInfo = s.InvoicePaymentInfo,
              });
            return await query.ToListAsync();
        }
        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Currencies_Create)]
        public async Task<CurrencyDto> Create(CurrencyDto input)
        {
            var isExist = await WorkScope.GetAll<Currency>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or Code already exist !"));

            await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<Currency>(input));
            return input;
        }
        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Currencies_Edit)]
        public async Task<CurrencyDto> Update(CurrencyDto input)
        {
            var currency = await WorkScope.GetAsync<Currency>(input.Id);

            var isExist = await WorkScope.GetAll<Currency>().AnyAsync(x => x.Id != input.Id && (x.Name == input.Name || x.Code == input.Code));

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or Code already exist !"));

            await WorkScope.UpdateAsync(ObjectMapper.Map<CurrencyDto, Currency>(input, currency));
            return input;
        }
        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Currencies_Delete)]
        public async Task Delete(long currencyId)
        {
            var hasProject = await WorkScope.GetAll<Project>().AnyAsync(x => x.CurrencyId == currencyId);
            if (hasProject)
                throw new UserFriendlyException("Currency already has a project !");

            await WorkScope.DeleteAsync<Currency>(currencyId);
        }
    }
}
