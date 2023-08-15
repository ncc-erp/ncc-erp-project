using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Newtonsoft.Json;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.PmReports.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProjectManagement.Configuration;
using ProjectManagement.Configuration.Dto;
using NccCore.Uitls;
using ProjectManagement.Services.PmReports;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace ProjectManagement.BackgroundWorkers
{
    public class InformPmWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly KomuService _komu;
        private readonly PmReportManager _pmReport;

        public InformPmWorker(AbpTimer timer, KomuService komuService,
           PmReportManager pmReportManager)
            : base(timer)
        {
            _komu = komuService;
            _pmReport = pmReportManager;
            Timer.Period = 60 * 1000;
        }

        [UnitOfWork]
        protected override async void DoWork()
        {
            // get time setting in database
            string json = SettingManager.GetSettingValueForApplication(AppSettingNames.InformPm);
            if (string.IsNullOrEmpty(json)) return;

            InformPmDto inform = JsonConvert.DeserializeObject<InformPmDto>(json);

            if (CheckSendTime(inform.CheckDateTimes))
                Inform(await _pmReport.GetPMsUnsentWeeklyReport(), inform.ChannelId);
        }

        private bool CheckSendTime(List<CheckDateTime> checks)
        {
            string now = DateTime.Now.ToString("HH:mm");
            if (checks.Any(c => c.IsCheck && (int)DateTimeUtils.GetNow().DayOfWeek == c.Day && c.Time == now))
                return true;
            return false;
        }

        private async void Inform(List<PMUnsentWeeklyReportDto> list, string channelId)
        {
            StringBuilder komuMessage = new StringBuilder();
            komuMessage.AppendLine($"List of projects that have not sent " +
                $"Weekly Report for {list.FirstOrDefault().WeeklyName} yet:?????");
            for (int i = 0; i < list.Count; i++)
            {
                komuMessage.AppendLine($"{i + 1}. {list[i].ProjectName} - " +
                $"PM: {GetTagPm(list[i])}?????");
            }
            string[] messages = komuMessage.ToString().Trim().Split("?????");
            await _komu.NotifyToChannelAwait(messages, channelId);
        }

        private string GetTagPm(PMUnsentWeeklyReportDto data) => "${" + data.EmailAddress.Trim().Split('@')[0] + "}";
        //=> data.KomuId.HasValue ? $"<@{data.KomuId}>" : $"**{data.EmailAddress}**";
    }
}
