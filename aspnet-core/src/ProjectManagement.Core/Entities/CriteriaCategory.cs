using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ProjectManagement.Entities
{
    public class CriteriaCategory : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
    }
}
