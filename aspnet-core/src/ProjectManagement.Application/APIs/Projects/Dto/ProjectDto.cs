using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Projects.Dto
{
    [AutoMapTo(typeof(Project))]
    public class ProjectDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectStatus Status { get; set; }
        public long? ClientId { get; set; }
        public long? CurrencyId { get; set; }
        public bool? IsCharge { get; set; }
        public ChargeType? ChargeType { get; set; }
        public long PmId { get; set; }
        public string BriefDescription { get; set; }
        public string DetailDescription { get; set; }
        public string TechnologyUsed { get; set; }
        public string TechnicalProblems { get; set; }
        public string OtherProblems { get; set; }
        public string NewKnowledge { get; set; }
        public bool RequireTimesheetFile { get; set; }
        public bool IsRequiredWeeklyReport { get; set; }
    }
}