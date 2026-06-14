using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Entities
{
    public class ProjectUserOnboardingDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
      
        [ForeignKey(nameof(ProjectUserOnboardingId))]
        public virtual ProjectUserOnboarding ProjectUserOnboarding { get; set; }
        public long ProjectUserOnboardingId { get; set; }

        [ForeignKey(nameof(OnboardingChecklistId))]
        public virtual OnboardingChecklist OnboardingChecklist { get; set; }
        public long OnboardingChecklistId { get; set; }

        public bool IsChecked { get; set; }
    }
}
