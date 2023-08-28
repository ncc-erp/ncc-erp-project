using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Clients.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManagement.Services.Timesheet;
using ProjectManagement.Utils;
using ProjectManagement.Services.Finance;

namespace ProjectManagement.APIs.Clients
{
    [AbpAuthorize]
    public class ClientAppService : ProjectManagementAppServiceBase
    {
        private readonly TimesheetService _timesheetService;
        private readonly FinfastService _finfastService;
        public ClientAppService(TimesheetService timesheetService, FinfastService finfastService)
        {
            _timesheetService = timesheetService;
            _finfastService = finfastService;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Clients)]
        public async Task<GridResult<ClientDto>> GetAllPaging(GridParam input)
        {
            var query = WorkScope.GetAll<Client>()
                .OrderByDescending(s => s.CreationTime)
                .Select(s => new ClientDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Code = s.Code,
                    Address = s.Address,
                    PaymentDueBy = s.PaymentDueBy,
                    InvoiceDateSetting = s.InvoiceDateSetting,
                    TransferFee = s.TransferFee,
                });
            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        public async Task<List<ClientDto>> GetAll()
        {
            var query = WorkScope.GetAll<Client>()
              .Select(s => new ClientDto
              {
                  Id = s.Id,
                  Name = s.Name,
                  Code = s.Code,
                  Address = s.Address,
              });
            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Clients_Create)]
        public async Task<string> Create(ClientDto input)
        {
            var isExist = await WorkScope.GetAll<Client>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code);

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or Code already exist !"));

            await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<Client>(input));

            var createCustomer = await _timesheetService.CreateCustomer(input.Name, input.Code, input.Address);
            var createAccount = await _finfastService.CreateAccount(input.Name, input.Code);
            var sb = new StringBuilder();
            sb.AppendLine(createCustomer);
            sb.AppendLine("");
            sb.AppendLine(createAccount);
            return sb.ToString();
        }
        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Clients_Edit)]
        public async Task<ClientDto> Update(ClientDto input)
        {
            var client = await WorkScope.GetAsync<Client>(input.Id);

            var isExist = await WorkScope.GetAll<Client>().AnyAsync(x => x.Id != input.Id && (x.Name == input.Name || x.Code == input.Code));

            if (isExist)
                throw new UserFriendlyException(String.Format("Name or Code already exist !"));

            await WorkScope.UpdateAsync(ObjectMapper.Map<ClientDto, Client>(input, client));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Clients_Delete)]
        public async Task Delete(long clientId)
        {
            var hasProject = await WorkScope.GetAll<Project>().AnyAsync(x => x.ClientId == clientId);
            if (hasProject)
                throw new UserFriendlyException("Client already has a project !");

            await WorkScope.DeleteAsync<Client>(clientId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllClient()
        {
            var clients = await WorkScope.GetAll<Client>().Select(x => new { Id = x.Id, Name = x.Name + " [" + x.Code + "]" }).ToListAsync();
            return new OkObjectResult(clients);
        }

        [HttpGet]
        public async Task<long> getIdClientByCodeNcc()
        {
            return await WorkScope.GetAll<Client>().Where(x => x.Code == "NCC").Select(x => x.Id).FirstOrDefaultAsync();
        }

        [HttpGet]
        public List<ValueTextDto> GetAllPaymentDueBy()
        {
            return CommonUtil.PaymentDueByList().Select(x => new ValueTextDto
            {
                Value = x.Key,
                Text = x.Value,
            }).ToList();
        }

        [HttpGet]
        public List<ValueTextDto> GetAllInvoiceDate()
        {
            return CommonUtil.InvoiceDateList().Select(x => new ValueTextDto
            {
                Value = Convert.ToInt16(x.Key),
                Text = x.Value,
            }).ToList();
        }
    }
}
