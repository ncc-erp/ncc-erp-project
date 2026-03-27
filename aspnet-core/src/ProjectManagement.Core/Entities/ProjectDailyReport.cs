using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProjectDailyReport : FullAuditedEntity<long>, IMayHaveTenant
    {
        [Column(TypeName = "date")]
        public DateTime Date { get; set; } 

        public string Content { get; set; }

        [ForeignKey(nameof(WeeklySummaryId))]
        public ProjectWeeklySummary WeeklySummary { get; set; }

        public long WeeklySummaryId { get; set; }

        public int? TenantId { get; set; }
    }
}
