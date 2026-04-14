using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.APIs.Public.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    [AutoMapTo(typeof(ProjectWeeklySummary))]
    public class GetProjectDailyMeetingsDto : EntityDto<long>
    {
        public string Summary { get; set; } 

        public long PMReportId { get; set; }

        public long ProjectId { get; set; }

        public List<MeetingReportCriteriaDetailDto> Criterias { get; set; }
    }
}
