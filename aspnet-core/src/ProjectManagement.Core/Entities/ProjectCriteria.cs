using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ProjectManagement.Entities
{
    public class ProjectCriteria : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string Guideline { get; set; }

        public string Name { get; set; }

        public int? TenantId { get; set; }
        public bool IsActive { get; set; }
    }
}