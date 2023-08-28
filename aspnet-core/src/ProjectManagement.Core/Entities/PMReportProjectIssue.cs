using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class PMReportProjectIssue : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(PMReportProjectId))]
        public PMReportProject PMReportProject { get; set; }
        public long PMReportProjectId { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        [MaxLength(10000)]
        public string Impact { get; set; }
        public IssueCritical Critical { get; set; }
        public ProjectIssueSource Source { get; set; }
        [MaxLength(10000)]
        public string Solution { get; set; }
        public string MeetingSolution { get; set; }// consider
        public PMReportProjectIssueStatus Status { get; set; }
    }
}