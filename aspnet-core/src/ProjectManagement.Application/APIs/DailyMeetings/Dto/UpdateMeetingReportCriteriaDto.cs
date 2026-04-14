using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    public class UpdateMeetingReportCriteriaDto : EntityDto<long>
    {
        public string CriteriaName { get; set; }

        public string Content { get; set; }

        public MeetingReportCriteriaStatus Status { get; set; }

    }
}
