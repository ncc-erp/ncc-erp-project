﻿using Abp.Authorization;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjectManagement.Authorization;
using ProjectManagement.Configuration.Dto;
using ProjectManagement.Services.CheckConnectDto;
using ProjectManagement.Services.Finance;
using ProjectManagement.Services.HRM;
using ProjectManagement.Services.Talent;
using ProjectManagement.Services.Timesheet;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using System.Text.Json;
using System.Collections.Generic;
using Castle.Core.Internal;
using OfficeOpenXml.FormulaParsing.Utilities;
using System.Text.RegularExpressions;
using System.Linq;
using NccCore.Extension;
using System.Threading.Tasks;
using System;
using Hangfire;
using ProjectManagement.Services.Komu;
using NccCore.Uitls;
using Hangfire.Storage;
using NccCore.IoC;
using ProjectManagement.Services.ProjectUserBills;
using ProjectManagement.EntityFrameworkCore;
using Abp.Domain.Repositories;
using ProjectManagement.Entities;
using Microsoft.Extensions.DependencyInjection;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System.Text;
using ProjectManagement.Manager.TimesheetManagers;

namespace ProjectManagement.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : ProjectManagementAppServiceBase, IConfigurationAppService
    {
        private static IConfiguration _appConfiguration;
        private readonly TalentService _talentService;
        private readonly HRMService _hrmService;
        private readonly FinfastService _finfastService;
        private readonly TimesheetService _timesheetService;
        private readonly string defaultTime = "17:00";
        private readonly Days defaultDayOfWeek = Days.Tue; // tuesday
        private readonly KomuService _komuService;
        private readonly IServiceProvider _provider;

        public ConfigurationAppService(IConfiguration appConfiguration,
            TalentService talentService,
            HRMService hrmService,
            FinfastService finfastService,
            TimesheetService timesheetService,
            KomuService komuService,
            IServiceProvider serviceProvider
            )
        {
            _appConfiguration = appConfiguration;
            _talentService = talentService;
            _hrmService = hrmService;
            _finfastService = finfastService;
            _timesheetService = timesheetService;
            _komuService = komuService;
            _provider = serviceProvider;
        }

        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }

        public async Task<string> GetGoogleClientAppId()
        {
            return await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientAppId);
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions)]
        public async Task<AppSettingDto> Get()
        {
            return new AppSettingDto
            {
                ClientAppId = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ClientAppId),
                SecurityCode = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.SecurityCode),
                FinanceUri = _appConfiguration.GetValue<string>("FinfastService:BaseAddress"),
                FinanceSecretCode = _appConfiguration.GetValue<string>("FinfastService:SecurityCode"),
                TimesheetUri = _appConfiguration.GetValue<string>("TimesheetService:BaseAddress"),
                TimesheetSecretCode = _appConfiguration.GetValue<string>("TimesheetService:SecurityCode"),
                AutoUpdateProjectInfoToTimesheetTool = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.AutoUpdateProjectInfoToTimesheetTool),
                CanSendDay = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.CanSendDay),
                CanSendHour = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.CanSendHour),
                ExpiredDay = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ExpiredDay),
                ExpiredHour = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ExpiredHour),
                KomuUrl = _appConfiguration.GetValue<string>("KomuService:BaseAddress"),
                NoticeToKomu = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.NoticeToKomu),
                KomuSecretCode = _appConfiguration.GetValue<string>("KomuService:SecurityCode"),
                KomuUserNames = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.KomuUserNames),
                UserBot = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.UserBot),
                PasswordBot = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.PasswordBot),
                HRMUri = _appConfiguration.GetValue<string>("HRMService:BaseAddress"),
                HRMSecretCode = _appConfiguration.GetValue<string>("HRMService:SecurityCode"),
                KomuRoom = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.KomuRoom),
                DefaultWorkingHours = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.DefaultWorkingHours),
                TrainingRequestChannel = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.TrainingRequestChannel),
                TalentUriBA = _appConfiguration.GetValue<string>("TalentService:BaseAddress"),
                TalentUriFE = _appConfiguration.GetValue<string>("TalentService:FEAddress"),
                TalentSecurityCode = _appConfiguration.GetValue<string>("TalentService:SecurityCode"),
                MaxCountHistory = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.MaxCountHistory),
                InformPm = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.InformPm),
                ActiveTimesheetProjectPeriod = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ActiveTimesheetProjectPeriod),
                CloseTimesheetNotification = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.CloseTimesheetNotification),
            };
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions_Edit)]
        public async Task<AppSettingDto> Change(AppSettingDto input)
        {
            if (string.IsNullOrEmpty(input.ClientAppId) ||
                string.IsNullOrEmpty(input.SecurityCode) ||
                string.IsNullOrEmpty(input.FinanceUri) ||
                string.IsNullOrEmpty(input.FinanceSecretCode) ||
                string.IsNullOrEmpty(input.TimesheetUri) ||
                string.IsNullOrEmpty(input.TimesheetSecretCode) ||
                string.IsNullOrEmpty(input.AutoUpdateProjectInfoToTimesheetTool) ||
                string.IsNullOrEmpty(input.CanSendDay) ||
                string.IsNullOrEmpty(input.CanSendHour) ||
                string.IsNullOrEmpty(input.ExpiredDay) ||
                string.IsNullOrEmpty(input.ExpiredHour) ||
                string.IsNullOrEmpty(input.KomuUserNames) ||
                string.IsNullOrEmpty(input.KomuUrl) ||
                string.IsNullOrEmpty(input.NoticeToKomu) ||
                string.IsNullOrEmpty(input.KomuSecretCode) ||
                string.IsNullOrEmpty(input.UserBot) ||
                string.IsNullOrEmpty(input.PasswordBot) ||
                string.IsNullOrEmpty(input.HRMUri) ||
                string.IsNullOrEmpty(input.HRMSecretCode) ||
                string.IsNullOrEmpty(input.KomuRoom) ||
                string.IsNullOrEmpty(input.DefaultWorkingHours) ||
                string.IsNullOrEmpty(input.MaxCountHistory) ||
                string.IsNullOrEmpty(input.InformPm) ||

                string.IsNullOrEmpty(input.MaxCountHistory) ||
                string.IsNullOrEmpty(input.ActiveTimesheetProjectPeriod) ||
                string.IsNullOrEmpty(input.CloseTimesheetNotification)
                )
            {
                throw new UserFriendlyException("All setting values need to be completed");
            }
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ClientAppId, input.ClientAppId);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.FinanceUri, input.FinanceUri);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.FinanceSecretCode, input.FinanceSecretCode);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.TimesheetUri, input.TimesheetUri);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.TimesheetSecretCode, input.TimesheetSecretCode);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.AutoUpdateProjectInfoToTimesheetTool, input.AutoUpdateProjectInfoToTimesheetTool);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.CanSendDay, input.CanSendDay);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.CanSendHour, input.CanSendHour);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ExpiredDay, input.ExpiredDay);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ExpiredHour, input.ExpiredHour);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.KomuUrl, input.KomuUrl);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.NoticeToKomu, input.NoticeToKomu);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.KomuSecretCode, input.KomuSecretCode);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.KomuUserNames, input.KomuUserNames);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.UserBot, input.UserBot);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.PasswordBot, input.PasswordBot);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.HRMUri, input.HRMUri);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.HRMSecretCode, input.HRMSecretCode);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.KomuRoom, input.KomuRoom);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.DefaultWorkingHours, input.DefaultWorkingHours);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.TrainingRequestChannel, input.TrainingRequestChannel);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.MaxCountHistory, input.MaxCountHistory);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InformPm, input.InformPm);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ActiveTimesheetProjectPeriod, input.ActiveTimesheetProjectPeriod);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.CloseTimesheetNotification, input.CloseTimesheetNotification);
            return input;
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions_Edit)]
        public async Task<ProjectSetting> ChangeProjectSetting(ProjectSetting input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.SecurityCode, input.SecurityCode);
            return input;
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<WeeklyReportSettingDto> GetTimeCountDown()
        {
            return new WeeklyReportSettingDto
            {
                TimeCountDown = Convert.ToInt32(await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.TimeCountDown))
            };
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions_WeeklyReportTime_Edit)]
        [HttpPost]
        public async Task<WeeklyReportSettingDto> SetTimeCountDown(WeeklyReportSettingDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.TimeCountDown, input.TimeCountDown.ToString());
            return input;
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions_Edit)]
        [HttpPost]
        public async Task<AuditScoreDto> SetAuditScore(AuditScoreDto input)
        {
            var json = JsonSerializer.Serialize(input);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.ScoreAudit, json);
            return input;
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions_ViewAuditScoreSetting)]
        [HttpGet]
        public async Task<AuditScoreDto> GetAuditScore()
        {
            var json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ScoreAudit);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonSerializer.Deserialize<AuditScoreDto>(json);
        }

        [AbpAuthorize(
            //PermissionNames.Admin_Configuartions_Edit,
            PermissionNames.WeeklyReport_ReportDetail_GuideLine_Update
            )]
        [HttpPost]
        public async Task<GuideLineDto> SetGuideLine(GuideLineDto input)
        {
            var json = JsonSerializer.Serialize(input);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.GuideLine, json);
            return input;
        }


        [AbpAuthorize(
           //PermissionNames.Admin_Configurations_ViewGuideLineSetting
           PermissionNames.WeeklyReport_ReportDetail_GuideLine_View
           )]
        [HttpGet]
        public async Task<GuideLineDto> GetGuideLine()
        {
            var allowViewGuideline = await PermissionChecker.IsGrantedAsync(PermissionNames.WeeklyReport_ReportDetail_GuideLine_View);
            if (!allowViewGuideline)
            {
                throw new UserFriendlyException("You are not allow to view this guideline!");
            }
            var json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.GuideLine);

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            return JsonSerializer.Deserialize<GuideLineDto>(json);
        }

        [HttpGet]
        public async Task<GetResultConnectDto> CheckConnectToTimesheet()
        {
            return await _timesheetService.CheckConnectToTimesheet();
        }

        [HttpGet]
        public async Task<GetResultConnectDto> CheckConnectToTalent()
        {
            return await _talentService.CheckConnectToTalent();
        }

        [HttpGet]
        public async Task<GetResultConnectDto> CheckConnectToFinfast()
        {
            return await _finfastService.CheckConnectToFinance();
        }

        [AbpAuthorize(
           PermissionNames.Admin_Configuartions_Edit
           )]
        [HttpPost]
        public async Task<InformPmDto> SetInformPm(InformPmDto input)
        {
            List<CheckDateTime> newList = new List<CheckDateTime>();
            ValidationDateTime(input.CheckDateTimes);
            var json = JsonSerializer.Serialize(input);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.InformPm, json);
            return input;
        }

        private void ValidationDateTime(List<CheckDateTime> checkDateTimes)
        {
            DateTime dtime;
            if (checkDateTimes.Any(c => !c.Time.HasValue() || c.Day < (int)Days.Sun || c.Day > (int)Days.Sat
                || !DateTime.TryParse(c.Time.Trim(), out dtime))) throw new UserFriendlyException("Time can not empty!");
            var listDuplicate = checkDateTimes.GroupBy(c => new { c.Time, c.Day }).ToList();
            if (listDuplicate.Count != checkDateTimes.Count) throw new UserFriendlyException("Time and day can not be duplicated!!");
        }

        [AbpAuthorize(
           PermissionNames.Admin_Configurations_ViewInformPmSetting
           )]
        [HttpGet]
        public async Task<InformPmDto> GetInformPm()
        {
            var json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.InformPm);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            var inform = JsonSerializer.Deserialize<InformPmDto>(json);
            return inform;
        }

        [AbpAuthorize(PermissionNames.Admin_Configuartions_Edit)]
        [HttpPost]
        public async Task<InformPmDto> SetCloseTimesheetNotification(InformPmDto input)
        {
            ValidationCloseTimesheet(input);
            var json = JsonSerializer.Serialize(input);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.CloseTimesheetNotification, json);
            return input;
        }

        private void ValidationCloseTimesheet(InformPmDto input)
        {
            if (input.CheckDateTimes.Any(t => t.IsCheck) && !input.ChannelId.HasValue())
                throw new UserFriendlyException("Channel Id is null!");
            var listDuplicate = input.CheckDateTimes.GroupBy(c => new { c.Time}).ToList();
            if (listDuplicate.Count != input.CheckDateTimes.Count) 
                throw new UserFriendlyException("Time can not be duplicated!!");
        }

        [HttpGet]
        public async Task<InformPmDto> GetCloseTimesheetNotification()
        {
            var json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.CloseTimesheetNotification);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            var closeNoti = JsonSerializer.Deserialize<InformPmDto>(json);
            return closeNoti;
        }

        [HttpGet]
        public async Task<string> GetActiveTimesheetProjectPeriod()
        {
            return await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.ActiveTimesheetProjectPeriod);
        }

        [HttpPost]
        public async Task<ChargeBillAccountDto> SetNotiAutoChargeBillAccount(ChargeBillAccountDto input)
        {
            try
            {
                var json = JsonSerializer.Serialize(input);
                await SettingManager.ChangeSettingForApplicationAsync(AppSettingNames.UpdateChargeStatusBillAccount, json);
                IEnumerable<string> recurringJobIds = JobStorage.Current.GetConnection().GetRecurringJobs()
                    .Where(j => j.Id.Contains(nameof(UpdateChargeBillAccount)) || j.Id.Contains(nameof(NotifyIsChargeUserBill)))
                    .Select(x => x.Id).ToList();
                foreach (var jobId in recurringJobIds)
                {
                    RecurringJob.RemoveIfExists(jobId);
                }
                // auto update
                var update = input.AutoUpdateBillAccount;
                if (update != null && update.IsCheck)
                {
                    var utcTime = DateTimeUtils.ConvertToUtcTime(-1, -1, update.Day, int.Parse(update.Time.Split(":")[0]), int.Parse(update.Time.Split(":")[1]));
                    var cronExpress = $"{utcTime.Minute} {utcTime.Hour} {utcTime.Day} * *";
                    RecurringJob.AddOrUpdate<ConfigurationAppService>($"{nameof(UpdateChargeBillAccount)}-{Guid.NewGuid()}",
                        s => s.UpdateChargeBillAccount(), cronExpress);
                }
                // noti user
                var noti = input.NotiUsers;
                if (noti != null && input.UserIds != null && noti.CheckDateTimes != null
                    && input.UserIds.Any() && noti.ChannelId.HasValue() && noti.CheckDateTimes.Any(c => c.IsCheck))
                {
                    var notiTimes = noti.CheckDateTimes.Where(c => c.IsCheck);
                    foreach (var time in notiTimes)
                    {
                        var utcTime = DateTimeUtils.ConvertToUtcTime(-1, -1, time.Day, int.Parse(time.Time.Split(":")[0]), int.Parse(time.Time.Split(":")[1]));
                        var cronExpress = $"{utcTime.Minute} {utcTime.Hour} {utcTime.Day} * *";
                        RecurringJob.AddOrUpdate<ConfigurationAppService>($"{nameof(NotifyIsChargeUserBill)}-{Guid.NewGuid()}",
                            s => s.NotifyIsChargeUserBill(input.UserIds, time, noti.ChannelId), cronExpress);
                    }
                }
            }
            catch (Exception)
            {
                throw new UserFriendlyException("Set auto update and noti bill account fail!");
            }
            return input;
        }

        [HttpGet]
        public async Task<ChargeBillAccountDto> GetNotiAutoChargeBillAccount()
        {
            var json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.UpdateChargeStatusBillAccount);
            if (string.IsNullOrEmpty(json))
            {
                return null;
            }
            var settingObject = JsonSerializer.Deserialize<ChargeBillAccountDto>(json);
            return settingObject;
        }

        public void UpdateChargeBillAccount()
        {
            using (var scope = _provider.CreateScope())
            {
                var WorkScope = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();

                var billAccounts = WorkScope.ProjectUserBills.Where(p => !p.IsDeleted && p.isActive
                 && p.EndTime != null && p.EndTime < DateTime.Now.Date && p.Project.Status != ProjectStatus.Closed);
                foreach (var item in billAccounts)
                {
                    item.isActive = false;
                }
                WorkScope.UpdateRange(billAccounts);
                WorkScope.SaveChanges();
            }
        }

        public async Task NotifyIsChargeUserBill(List<long> userIds, CheckDateTime time, string channelId)
        {
            using (var scope = _provider.CreateScope())
            {
                var WorkScope = scope.ServiceProvider.GetRequiredService<ProjectManagementDbContext>();
                var userEmails = WorkScope.ProjectUserBills
                       .Include(p => p.Project).Include(p => p.User)
                       .Where(u => !u.IsDeleted && userIds.Contains(u.UserId)
                       && u.EndTime != null && u.EndTime < DateTime.Now.Date && u.Project.Status != ProjectStatus.Closed
                       && u.Project.ProjectType != ProjectType.PRODUCT
                       && u.Project.ProjectType != ProjectType.NoBill
                       && u.Project.ProjectType != ProjectType.TRAINING
                       && u.isActive
                       )
                       .ToList()
                       .GroupBy(p => p.User.EmailAddress)
                       .Select(group => new EmailProjectDto
                       {
                           EmailAddress = group.Key,
                           Projects = group.Select(g => g.Project.Name).ToList()
                       });
                if (userEmails != null && userEmails.Any())
                {
                    var text = new StringBuilder();
                    text.AppendLine($"Update your bill account charge status: ");
                    var index = 0;
                    var splitChar = "%%%";
                    foreach (var item in userEmails)
                    {
                        string projectsString = string.Join(", ", item.Projects);
                        text.Append(++index + ". ${" + item.EmailAddress.Split("@")[0] + "}\t**Project:** " + projectsString + "." + splitChar);
                    }
                    var arr = text.ToString().Split(splitChar);
                    await _komuService.NotifyToChannelAwait(arr, channelId);
                }
            }
        }

    }
}