using Abp.Runtime.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProjectManagement.Services.CheckConnectDto;
using ProjectManagement.Services.HRM.Dto;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Services.HRM
{
    public class HRMService : BaseWebService
    {
        private const string serviceName = "HRMService";
        public HRMService(
            HttpClient httpClient, 
            IConfiguration configuration, 
            ILogger<HRMService> logger,
            IAbpSession abpSession
        ) : base(httpClient, configuration, logger, abpSession, serviceName)
        {
        }

        public async Task<List<AutoUpdateUserDto>> GetUserFromHRM()
        {
            return await GetAsync<List<AutoUpdateUserDto>>($"/api/services/app/ProjectManagement/GetAllUser");
        }
        public async Task<AutoUpdateUserDto> GetUserFromHRMByEmail(string email)
        {
            return await GetAsync<AutoUpdateUserDto>($"/api/services/app/ProjectManagement/GetUserByEmail?email={email}");
        }
    }
}
