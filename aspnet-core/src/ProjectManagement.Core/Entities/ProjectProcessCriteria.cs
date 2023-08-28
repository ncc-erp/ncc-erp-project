using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ProjectProcessCriteria : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long ProcessCriteriaId { get; set; }
        [ForeignKey(nameof(ProcessCriteriaId))]
        public ProcessCriteria ProcessCriteria { get; set; }
        public long ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public string Note { get; set; }
        public Applicable Applicable { get; set; }
    }
}