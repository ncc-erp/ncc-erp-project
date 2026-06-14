using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProjectWeeklySummary : FullAuditedEntity<long>, IMayHaveTenant
    {
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
