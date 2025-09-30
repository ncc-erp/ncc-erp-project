using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class OnboardUserProject: FullAuditedEntity<long>
    {
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(OnboardStepId))]
        public OnboardSteps OnboardSteps { get; set; }
        public int OnboardStepId { get; set; }
        public bool IsCompleted { get; set; }

    }
}
