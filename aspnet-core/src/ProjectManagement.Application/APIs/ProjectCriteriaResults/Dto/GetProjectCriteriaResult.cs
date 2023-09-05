using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectCriteriaResults
{
    [AutoMapTo(typeof(ProjectCriteriaResult))]
    public class GetProjectCriteriaResultDto : EntityDto<long>
    {
        public string CriteriaName { get; set; }

        public string Guideline { get; set; }

        public string Note { get; set; }

        public long PMReportId { get; set; }

        public long ProjectId { get; set; }

        public ProjectEnum.ProjectCriteriaResultStatus Status { get; set; }

        public long ProjectCriteriaId { get; set; }
    }
}