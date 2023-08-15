using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Entities
{
    public class Criteria : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public long CriteriaCategoryId { get; set; }
        [ForeignKey(nameof(CriteriaCategoryId))]
        public CriteriaCategory CriteriaCategory { get; set; }
        public int Weight { get; set; }
        public string Note { get; set; }
    }
}
