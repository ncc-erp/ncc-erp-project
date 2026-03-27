using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NccCore.DataExport;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ProjectManagement.APIs.Public.Dto;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Services.CheckConnectDto;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.UploadFilesService;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Public
{
    public class PublicAppService : ProjectManagementAppServiceBase
    {
        private readonly IConfiguration _appConfiguration;
        private readonly ResourceManager resourceManager;
        private readonly UploadFileService _uploadFileService;
        protected IHttpContextAccessor _httpContextAccessor { get; set; }

        public PublicAppService(ResourceManager resourceManager, IConfiguration appConfiguration, IHttpContextAccessor httpContextAccessor, UploadFileService uploadFileService)
        {
            this.resourceManager = resourceManager;
            this._appConfiguration = appConfiguration;
            _httpContextAccessor = httpContextAccessor;
            _uploadFileService = uploadFileService;
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

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode != securityCodeHeader)
            {
                return new BadRequestObjectResult("You do not have permission to retrieve projects.");
            }
            var query = WorkScope.GetAll<Project>()
               .Where(p => p.ProjectType != ProjectType.TRAINING && p.ProjectType != ProjectType.PRODUCT)
               .Select(p => p);
            var result = await query.ToListAsync();
            return new OkObjectResult(result);
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<Project>>> GetAllClients()
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode != securityCodeHeader)
            {
                return new BadRequestObjectResult("You do not have permission to retrieve clients.");
            }
            var query = WorkScope.GetAll<Client>()
               .Where(p => !p.IsDeleted)
               .Select(p => p);
            var result = await query.ToListAsync();
            return new OkObjectResult(result);
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
                .Where(x => emails.Contains(x.User.EmailAddress))
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
            var PMs = WorkScope.GetAll<Project>()
                .Where(x => emails.Contains(x.PM.EmailAddress))
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<LateReportPMDto> GetWeeklyReportSendTimeSlots()
        {
            // Get the active weekly PMReport
            var activePMReport = await WorkScope.GetAll<PMReport>()
                .Where(x => x.IsActive && x.Type == PMReportType.Weekly)
                .FirstOrDefaultAsync();

            if (activePMReport == null)
                throw new UserFriendlyException("No active weekly PMReport found");

            var activePMReportId = activePMReport.Id;

            // Get all PMReportProject entries for the active PMReport
            // using a join to get User information
            // Filter by projects that require weekly reports
            var reportData = await (from r in WorkScope.GetAll<PMReportProject>().Where(x => x.Project.IsRequiredWeeklyReport == true)
                                    join u in WorkScope.GetAll<User>() on r.PMId equals u.Id
                                    where r.PMReportId == activePMReportId
                                    select new
                                    {
                                        r.PMId,
                                        r.ProjectId,
                                        r.TimeSendReport,
                                        UserName = u.UserName,
                                        EmailAddress = u.EmailAddress
                                    }).ToListAsync();

            // Group the report data by PMId and ProjectId
            var from15To17 = reportData
                .Where(x => x.TimeSendReport != null && x.TimeSendReport.Value.Hour >= 15 && x.TimeSendReport.Value.Hour < 17)
                .Select(x => new PMReportDto
                {
                    PMId = x.PMId,
                    ProjectId = x.ProjectId,
                    TimeSendReport = x.TimeSendReport,
                    UserName = x.UserName,
                    EmailAddress = x.EmailAddress
                })
                .ToList();

            var after17 = reportData
                .Where(x => x.TimeSendReport == null || x.TimeSendReport.Value.Hour >= 17)
                .Select(x => new PMReportDto
                {
                    PMId = x.PMId,
                    ProjectId = x.ProjectId,
                    TimeSendReport = x.TimeSendReport,
                    UserName = x.UserName,
                    EmailAddress = x.EmailAddress
                })
                .ToList();

            return new LateReportPMDto
            {
                Between3To5PM = from15To17,
                After5PMOrMissing = after17
            };
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<PunishmentRecordsDto>> GetPunishments(int month, int year)
        {
            var filePath = await WorkScope.GetAll<Punishment>()
                 .Where(s => s.Month == month && s.Year == year)
                 .Select(s => s.FilePath)
                 .FirstOrDefaultAsync();
         
            if(string.IsNullOrEmpty(filePath)) 
                throw new UserFriendlyException("File path not found");

            var data = await _uploadFileService.DownloadPunishmentFileAsync(filePath);
            if (data == null || data.Length == 0)
                throw new UserFriendlyException("File data is empty");

            var rawExcelData = new List<PunishmentRecordsDto>();

            using (var stream = new MemoryStream(data))
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension?.Rows ?? 0;

                for (int row = 2; row <= rowCount; row++)
                {
                    var email = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                    if (string.IsNullOrEmpty(email))
                        continue;

                    var amountVal = Convert.ToDecimal(worksheet.Cells[row, 6].Value ?? 0);

                    var finalAmount = (amountVal > 0 && amountVal < 1000) ? amountVal * 1000 : amountVal;

                    rawExcelData.Add(new PunishmentRecordsDto
                    {
                        Email = email,
                        Date = worksheet.Cells[row, 4].GetValue<DateTime>().ToString("yyyy-MM-dd"),
                        Reason = worksheet.Cells[row, 5].Value?.ToString(),
                        Amount = finalAmount
                    });
                }
            }

            var emailList = rawExcelData.Select(x => x.Email).Distinct().ToList();

            var userMap = await WorkScope.GetAll<User>()
                .Where(u => emailList.Contains(u.EmailAddress))
                .Select(u => new { u.EmailAddress, u.MezonUserId })
                .ToDictionaryAsync(u => u.EmailAddress, u => u.MezonUserId);

            foreach (var record in rawExcelData)
            {
                if (userMap.TryGetValue(record.Email, out var mezonId))
                {
                    record.MezonId = mezonId;
                }
            }

            return rawExcelData;
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetMyProject(string mezonUserId)
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode != securityCodeHeader)
            {
                return new BadRequestObjectResult("You do not have permission to retrieve projects.");
            }
            if (string.IsNullOrEmpty(mezonUserId))
            {
                return new BadRequestObjectResult("Mezon User Id is required.");
            }
            var query = WorkScope.GetAll<Project>()
                .Where(p => p.PM.MezonUserId == mezonUserId)
                .Where(p => p.ProjectType != ProjectType.TRAINING && p.ProjectType != ProjectType.PRODUCT && p.Status == ProjectStatus.InProgress)
                .Select(p => new
                {
                    project_id = p.Code,
                    project_name = p.Name
                });
            var result = await query.ToListAsync();
            return new OkObjectResult(result);
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> UpdateDailyMeetings([FromBody] WeeklyReportDto input)
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode != securityCodeHeader)
            {
                return new BadRequestObjectResult("You do not have permission to retrieve projects.");
            }

            var project = await WorkScope.GetAll<Project>()
                .FirstOrDefaultAsync(x => x.Id == input.ProjectId);
            if (project == null)
            {
                return new BadRequestObjectResult($"Project code {input.ProjectId} not found.");
            }
            var activeReport = await WorkScope.GetAll<PMReport>()
                .FirstOrDefaultAsync(x => x.IsActive && x.Type == PMReportType.Weekly);
            if (activeReport == null)
            {
                return new BadRequestObjectResult("No active weekly report found to sync.");
            }
            var reportProject = await WorkScope.GetAll<PMReportProject>()
                .FirstOrDefaultAsync(x => x.ProjectId == project.Id && x.PMReportId == activeReport.Id);
            if (reportProject == null)
            {
                return new BadRequestObjectResult("This project is not in the current weekly report.");
            }

            var existingSummary = await WorkScope.GetAll<ProjectWeeklySummary>()
                .FirstOrDefaultAsync(x => x.ProjectId == project.Id && x.PMReportId == activeReport.Id);

            if (existingSummary != null)
            {
                existingSummary.OverallSummary = input.OverallSummary;
                await WorkScope.UpdateAsync(existingSummary);
            }
            else
            {
                existingSummary = new ProjectWeeklySummary
                {
                    ProjectId = project.Id,
                    PMReportId = activeReport.Id,
                    OverallSummary = input.OverallSummary,
                    TenantId = AbpSession.TenantId
                };
                existingSummary.Id = await WorkScope.InsertAndGetIdAsync(existingSummary);
            }

            if (input.DailyReports?.Any() == true)
            {
                var currentDailies = await WorkScope.GetAll<ProjectDailyReport>()
                    .Where(x => x.WeeklySummaryId == existingSummary.Id)
                    .ToListAsync();

                foreach (var dto in input.DailyReports)
                {
                    var reportDate = dto.Date.Date;
                    var match = currentDailies.FirstOrDefault(x => x.Date.Date == reportDate);

                    if (match != null)
                    {
                        if (match.Content != dto.Content)
                        {
                            match.Content = dto.Content;
                            await WorkScope.UpdateAsync(match);
                        }
                    }
                    else
                    {
                        await WorkScope.InsertAsync(new ProjectDailyReport
                        {
                            WeeklySummaryId = existingSummary.Id,
                            Date = reportDate,
                            Content = dto.Content,
                            TenantId = AbpSession.TenantId
                        });
                    }
                }
            }
            return new OkObjectResult(new { message = "Sync summary successfully!" });
        }
    }
}
