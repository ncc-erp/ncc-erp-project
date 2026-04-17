using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.DailyMeetings.Dto
{
    [AutoMapFrom(typeof(MeetingReportCriteria))]
    public class MeetingReportCriteriaDetailDto : EntityDto<long>
    {
        public string CriteriaName { get; set; }

        public string Content { get; set; }

        public MeetingReportCriteriaStatus Status { get; set; }

    }
}
