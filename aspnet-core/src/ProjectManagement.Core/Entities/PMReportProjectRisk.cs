using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class PMReportProjectRisk :FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(PMReportProjectId))]
        public PMReportProject PMReportProject { get; set; }
        public long PMReportProjectId { get; set; }
        [MaxLength(10000)]
        public string Risk { get; set; }
        [MaxLength(10000)]
        public string Impact { get; set; }
        public Priority Priority { get; set; }
        [MaxLength(10000)]
        public string Solution { get; set; }
        public PMReportProjectRiskStatus Status { get; set; }
    }
}
