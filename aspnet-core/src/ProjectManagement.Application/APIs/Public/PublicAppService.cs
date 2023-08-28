using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectManagement.APIs.Public.Dto;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Services.CheckConnectDto;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Public
{
    public class PublicAppService : ProjectManagementAppServiceBase
    {
        private readonly IConfiguration _appConfiguration;
        private readonly ResourceManager resourceManager;
        protected IHttpContextAccessor _httpContextAccessor { get; set; }

        public PublicAppService(ResourceManager resourceManager, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor)
        {
            this.resourceManager = resourceManager;
            this._appConfiguration = appConfiguration;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<GetURIDto> GetConfigUri()
        {
            return new GetURIDto
            {
                TimesheetURI = _appConfiguration.GetValue<string>("TimesheetService:BaseAddress"),
                GoogleClientAppId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientAppId)
            };
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public List<UserInOutProject> GetTempUsersInOutProjectHistory(string projectCode)
        {
            var projectId = GetProjectIdByCode(projectCode);

            if (projectId == default)
                throw new UserFriendlyException("Project not exist !");

            var tempProjectUsers = WorkScope.GetAll<ProjectUser>()
                    .Where(s => s.ProjectId == projectId)
                    .Where(s => s.User.UserType != UserType.FakeUser)
                    .Where(s => s.IsPool)
                    .Where(s => s.Status != ProjectUserStatus.Future)
                    .Select(s => new
                    {
                        s.User.EmailAddress,
                        s.UserId,
                        s.StartTime,
                        s.AllocatePercentage
                    })
                    .ToList();

            var resultList = tempProjectUsers
                .GroupBy(s => new { s.EmailAddress, s.UserId })
                .Select(s =>
             new UserInOutProject
             {
                 EmailAddress = s.Key.EmailAddress,
                 ListTimeInOut = s.Select(x => new TimeJoinOut
                 {
                     DateAt = x.StartTime.Date,
                     IsJoin = x.AllocatePercentage > 0
                 }).OrderBy(x => x.DateAt).ToList()
             }).ToList();

            return resultList;
        }

        private long GetProjectIdByCode(string projectCode)
        {
            return WorkScope.GetAll<Project>()
                .Where(x => x.Code.ToUpper() == projectCode.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public List<string> GetCurrentTempEmailsInProject(string projectCode)
        {
            var projectId = GetProjectIdByCode(projectCode);

            if (projectId == default)
                throw new UserFriendlyException($"Project with code {projectCode} is not exist!");

            var emails = WorkScope.GetAll<ProjectUser>()
                    .Where(s => s.ProjectId == projectId)
                    .Where(s => s.User.UserType != UserType.FakeUser)
                    .Where(s => s.Status == ProjectUserStatus.Present)
                    .Where(s => s.AllocatePercentage > 0)
                    .Where(s => s.IsPool)
                    .OrderByDescending(s => s.StartTime)
                    .Select(s => s.User.EmailAddress)
                    .Distinct()
                    .ToList();

            return emails;
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public List<CurrentTempProjectUserDto> GetAllCurrentTempProjectUser()
        {
            var results = WorkScope.GetAll<ProjectUser>()
                    .Where(s => s.User.UserType != UserType.FakeUser)
                    .Where(s => s.Status == ProjectUserStatus.Present)
                    .Where(s => s.AllocatePercentage > 0)
                    .Where(s => s.IsPool)
                    .OrderByDescending(s => s.StartTime)
                    .Select(s => new CurrentTempProjectUserDto
                    {
                        EmailAddress = s.User.EmailAddress,
                        ProjectCode = s.Project.Code
                    }).ToList();

            return results;
        }

        [HttpGet]
        public List<BaseUserInfo> GetAllUser()
        {
            return WorkScope.GetAll<User>().Select(x => new BaseUserInfo()
            {
                UserType = x.UserType,
                FullName = x.FullName,
                BranchName = x.Branch.Name,
                EmailAddress = x.EmailAddress,
                AvatarPath = x.AvatarPath
            }).ToList();
        }

        [HttpGet]
        public List<PMOfUserDto> GetPMOfUser(string email)
        {
            var userId = GetUserIdByEmail(email);

            if (userId == default)
            {
                Logger.Info($"No user by email: {email}");
                return null;
            }

            return resourceManager.QueryPMOfUser(userId);
        }


        [HttpGet]
        [AbpAllowAnonymous]
        public GetResultConnectDto CheckConnect()
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            var result = new GetResultConnectDto();
            if (secretCode != securityCodeHeader)
            {
                result.IsConnected = false;
                result.Message = $"SecretCode does not match: " + securityCodeHeader + " != ***" + secretCode.Substring(secretCode.Length - 3);
                return result;
            }
            result.IsConnected = true;
            result.Message = "Connected";
            return result;
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public List<PMsOfUsersDto> GetListPMsOfUsers(List<string> emails)
        {
            var result = WorkScope.GetAll<ProjectUser>()
                .Where(x => emails.Contains(x.User.EmailAddress) )
                .Where(s => s.Status == ProjectUserStatus.Present && s.AllocatePercentage > 0)
                            .Where(s => s.Project.Status == ProjectStatus.InProgress)
                .Select(x => new PMsOfUsersDto
                {
                    UserEmail = x.User.EmailAddress,
                    PMEmail = x.Project.PM.EmailAddress,
                    ProjectCode = x.Project.Code,
                    PMFullName = x.Project.PM.FullName,
                    ProjectName = x.Project.Name,
                }).ToList();
            var PMs= WorkScope.GetAll<Project>()
                .Where(x=>emails.Contains(x.PM.EmailAddress))
                .Where(s => s.Status == ProjectStatus.InProgress)
                .Select(x => new PMsOfUsersDto
                {
                    UserEmail = x.PM.EmailAddress,
                    PMEmail = x.PM.EmailAddress,
                    ProjectCode = x.Code,
                    PMFullName = x.PM.FullName,
                    ProjectName = x.Name,
                }).ToList();
            result.AddRange(PMs);
            return result.Distinct().ToList();
        }
    }
}