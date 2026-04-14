using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using System.Text;

namespace ProjectManagement.Entities
{
    public class MeetingReportCriteria : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string SectionName { get; set; }
        public MeetingReportCriteriaStatus Status { get; set; } = MeetingReportCriteriaStatus.Green;
        public string CriteriaName { get; set; }
        public string Content { get; set; }
        public long WeeklySummaryId { get; set; }
        
        [ForeignKey(nameof(WeeklySummaryId))]
        public ProjectWeeklySummary WeeklySummary { get; set; }
        public int? TenantId { get; set; }
    }
}
