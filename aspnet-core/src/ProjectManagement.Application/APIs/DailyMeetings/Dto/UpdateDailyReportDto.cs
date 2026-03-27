using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    public class UpdateDailyReportDto : EntityDto<long>
    {
        public string Content { get; set; }
    }
}
