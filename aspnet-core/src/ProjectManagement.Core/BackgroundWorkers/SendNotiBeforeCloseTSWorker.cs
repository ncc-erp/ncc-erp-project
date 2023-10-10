using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using NccCore.Uitls;
using Newtonsoft.Json;
using ProjectManagement.BackgroundWorkers.Dtos;
using ProjectManagement.Configuration;
using ProjectManagement.Services.Komu;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.BackgroundWorkers
{
    public class SendNotiBeforeCloseTSWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly KomuService _komu;

        public SendNotiBeforeCloseTSWorker(
            AbpTimer timer,
            KomuService komuService) :base(timer)
        {
            _komu = komuService;
            Timer.Period = 30 * 1000;
        }

        [UnitOfWork]
        protected override async void DoWork()
        {
            string json = await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.CloseTimesheetNotification);
            if (string.IsNullOrEmpty(json)) return;

            CloseNotificationDto closeNoti = JsonConvert.DeserializeObject<CloseNotificationDto>(json);

            if (CheckSendTime(closeNoti))
                await SendNotification("message here", closeNoti.ChannelId);
        }

        private async Task SendNotification(string message, string channelId)
        {

        }

        private bool CheckSendTime(CloseNotificationDto checks)
        {
            string now = DateTime.Now.ToString("HH:mm");

            foreach (var checkDateTime in checks.CheckDateTimes)
            {
                if (DateTime.TryParseExact(checks.CloseTime, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime closeTime))
                {
                    DateTime adjustedCloseTime = closeTime.AddHours(-checkDateTime.TimeSpan);

                    if (checkDateTime.IsCheck && checkDateTime.TimeSpan >= 0
                        && (int)DateTime.Now.DayOfWeek == checks.CloseDay && adjustedCloseTime.ToString("HH:mm") == now)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
