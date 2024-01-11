using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.BackgroundWorkers.Dtos
{
    public class CloseNotificationDto
    {
        public string ChannelId { get; set; }
        public List<CheckDateTimeDto> CheckDateTimes { get; set; }
    }

    public class CheckDateTimeDto
    {
        public bool IsCheck { get; set; }
        public string Time { get; set; }
        public int Day { get; set; }
    }
}
