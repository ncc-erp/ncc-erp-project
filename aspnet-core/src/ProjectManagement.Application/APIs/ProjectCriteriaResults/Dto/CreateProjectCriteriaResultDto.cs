using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectCriteriaResults.Dto
{
    [AutoMapTo(typeof(ProjectCriteriaResult))]
    public class CreateProjectCriteriaResultDto : EntityDto<long>
    {
        public string Note { get; set; }

        public long PMReportId { get; set; }

        public long ProjectCriteriaId { get; set; }

        public long ProjectId { get; set; }

        public ProjectCriteriaResultStatus Status { get; set; }

        public int? TenantId { get; set; }
    }
}