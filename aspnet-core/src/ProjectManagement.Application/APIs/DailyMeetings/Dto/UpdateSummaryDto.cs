using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    public class UpdateSummaryDto : EntityDto<long>
    {
        public string Summary { get; set; }
    }
}
