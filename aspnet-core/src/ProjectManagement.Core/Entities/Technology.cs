using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Entities
{
    public class Technology : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Color { get; set; }

    }
}
