using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.BackgroundWorkers.Dtos
{
    public class CloseNotificationDto
    {
        public string ChannelId { get; set; }
        public string CloseTime { get; set; }
        public int CloseDay { get; set; }
        public List<CheckDateTimeDto> CheckDateTimes { get; set; }
    }
}
