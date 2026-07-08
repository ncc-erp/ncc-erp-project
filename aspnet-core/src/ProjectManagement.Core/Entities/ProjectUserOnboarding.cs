using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ProjectUserOnboarding : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [ForeignKey(nameof(ProjectUserId))]
        public virtual ProjectUser ProjectUser { get; set; }

        public long ProjectUserId { get; set; }

        public ProjectUserOnboardingStatus Status { get; set; }

        public DateTime? SentRequestTime { get; set; }

        public DateTime? ConfirmedTime { get; set; }
        public string Note { get; set; }
        public DateTime? OnboardDate { get; set; }
        public virtual ICollection<ProjectUserOnboardingDetail> ProjectUserOnboardingDetails { get; set; }

    }
}
