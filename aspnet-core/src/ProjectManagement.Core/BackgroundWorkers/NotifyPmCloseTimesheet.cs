using Abp.Application.Services;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Logging;
using NccCore.Extension;
using NccCore.IoC;
using Newtonsoft.Json;
using ProjectManagement.BackgroundJobs;
using ProjectManagement.BackgroundWorkers.Dtos;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Manager.TimesheetManagers;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.BackgroundWorkers
{
    public class NotifyPmCloseTimesheet : PeriodicBackgroundWorkerBase, ISingletonDependency, IApplicationService
    {
        private readonly KomuService _komu;
        private readonly IWorkScope _workScope;
        private readonly ILogger<NotifyPmCloseTimesheet> _logger;

        public NotifyPmCloseTimesheet(
            AbpTimer timer,
            KomuService komuService, IWorkScope workScope, ILogger<NotifyPmCloseTimesheet> logger) : base(timer)
        {
            _komu = komuService;
            Timer.Period = (60 - DateTime.Now.Second) * 1000;
            _workScope = workScope;
            _logger = logger;
        }

        [UnitOfWork]
        protected override async void DoWork()
        {
            Timer.Period = 60 * 1000;
            if (GetAllTimesheetCloseTimeInJob().Count == 0) return;
            string closeTimesheet = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.CloseTimesheetNotification);
            if (string.IsNullOrEmpty(closeTimesheet)) return;

            var closeNoti = JsonConvert.DeserializeObject<CloseNotificationDto>(closeTimesheet);
            if (!closeNoti.ChannelId.HasValue() || closeNoti.CheckDateTimes.All(e => !e.IsCheck)) return;

            CompareNotifyTime(closeNoti);
        }

        private void CompareNotifyTime(CloseNotificationDto closeNoti)
        {
            var timesheetCloseInJob = GetAllTimesheetCloseTimeInJob();
            if (timesheetCloseInJob == null) return;
            foreach (var time in closeNoti.CheckDateTimes.Where(c => c.IsCheck))
            {
                var targetTime = DateTime.Now.AddHours(double.Parse(time.Time.Split(':')[0]))
                    .AddMinutes(double.Parse(time.Time.Split(':')[1]));
                foreach (var job in timesheetCloseInJob)
                {
                    if (DateTime.Compare(
                        new DateTime(targetTime.Year, targetTime.Month, targetTime.Day, targetTime.Hour, targetTime.Minute, 0),
                        new DateTime(job.Value.Year, job.Value.Month, job.Value.Day, job.Value.Hour, job.Value.Minute, 0)) == 0)
                        SendNotifyPmAsync(job.Key, job.Value, closeNoti.ChannelId);
                }

            }
        }

        private async Task SendNotifyPmAsync(long timeSheetId, DateTime closeTime, string channelId)
        {
            var listPm = GetPmEmailProjectByTimesheetId(timeSheetId);
            StringBuilder komuMessage = new StringBuilder();
            komuMessage.AppendLine($"Timesheet: **{GetTimesheetNameById(timeSheetId)}** " +
                $"will close at **{closeTime.ToString("dd-MM-yyyy HH:mm")}**:");
            for (int i = 0; i < listPm.Count; i++)
            {
                komuMessage.Append((i + 1) + ". PM: ${" + listPm.ElementAt(i).Key.Trim().Split('@')[0] + "}\t**Project name**: ");
                var listProjectNames = listPm.ElementAt(i).Value;
                komuMessage.Append(listProjectNames + ".###");
            }

            await _komu.NotifyToChannelAwait(komuMessage.ToString().Split("###"), channelId);
        }

        private string GetTimesheetNameById(long timeSheetId)
        {
            return _workScope.Get<Timesheet>(timeSheetId).Name;
        }

        private Dictionary<long, DateTime> GetAllTimesheetCloseTimeInJob()
        {
            var jobType = typeof(CloseTimeSheetBackgroundJob).FullName;
            return _workScope.GetAll<BackgroundJobInfo>()
                .Where(job => job.JobType.Contains(jobType))
                .Select(job => new
                {
                    TimesheetId = JsonConvert.DeserializeObject<TimesheetBGJDto>(job.JobArgs).TimesheetId,
                    Time = job.NextTryTime
                }).ToDictionary(job => job.TimesheetId, job => job.Time);
        }

        private Dictionary<string, string> GetPmEmailProjectByTimesheetId(long timesheetId)
        {
            return _workScope.GetAll<TimesheetProject>().Where(t => t.TimesheetId == timesheetId)
                .Select(tp => new
                {
                    pmEmail = tp.Project.PM.EmailAddress,
                    projectName = tp.Project.Name,
                }).ToList().GroupBy(tp => tp.pmEmail)
                .ToDictionary(group => group.Key, group => string.Join(",", group.Select(g => g.projectName)));
        }
    }
}
