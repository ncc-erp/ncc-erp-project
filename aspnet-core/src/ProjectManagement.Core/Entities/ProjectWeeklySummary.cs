using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ProjectWeeklySummary : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string SectionName { get; set; }
        public MeetingReportCriteriaStatus Status { get; set; } = MeetingReportCriteriaStatus.Green;

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }

        [ForeignKey(nameof(PMReportId))]
        public PMReport PMReport { get; set; }
        public long PMReportId { get; set; }

        public virtual ICollection<MeetingReportCriteria> DailyReports { get; set; }
        public int? TenantId { get; set; }

    }
}
