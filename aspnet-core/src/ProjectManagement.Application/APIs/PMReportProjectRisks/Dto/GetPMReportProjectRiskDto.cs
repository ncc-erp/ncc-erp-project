using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjectRisks.Dto
{
    public class GetPMReportProjectRiskDto
    {
        public long Id { get; set; }
        public string Risk { get; set; }
        public long PMReportProjectId { get; set; }
        public string Impact { get; set; }
        public string Solution { get; set; }
        public PMReportProjectRiskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Priority Priority { get; set; }
    }
}