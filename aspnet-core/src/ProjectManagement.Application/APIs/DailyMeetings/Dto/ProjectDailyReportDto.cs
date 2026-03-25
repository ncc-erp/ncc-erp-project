using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    [AutoMapTo(typeof(ProjectDailyReport))]
    public class ProjectDailyReportDto : EntityDto<long>
    {
        public DateTime Date { get; set; }

        public string Content { get; set; }

        public long WeeklySummaryId { get; set; }

    }
}
