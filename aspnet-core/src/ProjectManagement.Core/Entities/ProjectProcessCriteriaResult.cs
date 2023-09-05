using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ProjectProcessCriteriaResult : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long ProjectProcessResultId { get; set; }

        [ForeignKey(nameof(ProjectProcessResultId))]
        public virtual ProjectProcessResult ProjectProcessResult { get; set; }
        public long ProjectId { get; set; }
        public long ProcessCriteriaId { get; set; }
        [ForeignKey(nameof(ProcessCriteriaId))]
        public ProcessCriteria ProcessCriteria { get; set; }
        public NCStatus Status { get; set; }
        public int Score { get; set; }
        public string Note { get; set; }


    }
}
