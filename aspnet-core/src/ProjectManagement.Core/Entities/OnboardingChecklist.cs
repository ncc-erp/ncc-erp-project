using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Entities
{
    public class OnboardingChecklist : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [Required]
        public string Label { get; set; }

        public string DetailsJson { get; set; }

        public int Order { get; set; }
    }
}
