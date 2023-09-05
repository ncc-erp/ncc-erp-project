using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace ProjectManagement.APIs.PMReportProjectRisks.Dto
{
    [AutoMapTo(typeof(PMReportProjectRisk))]
    public class PMReportProjectRiskDto :EntityDto<long>
    {
        public long PMReportProjectId { get; set; }
        public string Risk { get; set; }
        public string Impact { get; set; }
        public Priority Priority { get; set; }
        public string Solution { get; set; }
        public PMReportProjectRiskStatus Status { get; set; }
    }
}