using Abp.Configuration;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProjectManagement.Configuration;
using ProjectManagement.Services.CheckConnectDto;
using ProjectManagement.Services.Finance.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Services.Finance
{
    public class FinfastService : BaseWebService
    {
        private const string serviceName = "FinfastService";
        public FinfastService(
            HttpClient httpClient, 
            ILogger<FinfastService> logger, 
            IConfiguration configuration,
            IAbpSession abpSession
        ) : base(httpClient, configuration, logger, abpSession, serviceName)
        {
        }
        public async Task<CreateInvoiceDto> CreateInvoiceToFinance(CreateInvoiceDto input)
        {
            return await PostAsync<CreateInvoiceDto>($"api/services/app/ProjectManagement/CreateInvoice", input);
        }
        public async Task<ResponseResultProjectDto> CreateAllInvoices(List<CreateInvoiceDto> input)
        {
            return await PostAsync<ResponseResultProjectDto>("api/services/app/ProjectTool/CreateAllInvoices", input);
        }
        public async Task<string> CreateAccount(string name, string code)
        {
            var item = new
            {
                Name = name,
                Code = code
            };
            return await PostAsync<string>($"/api/services/app/ProjectManagement/CreateAccount", item);
        }

        public async Task<GetResultConnectDto> CheckConnectToFinance()
        {
            var res = await GetAsync<GetResultConnectDto>($"api/services/app/Public/CheckConnect");
            if (res == null)
            {
                return new GetResultConnectDto
                {
                    IsConnected = false,
                    Message = "Can not connect to Finfast"
                };
            }
            if (res.IsConnected == false)
            {
                return new GetResultConnectDto
                {
                    IsConnected = false,
                    Message = res.Message
                };
            }
            return res;
        }
    }
}
