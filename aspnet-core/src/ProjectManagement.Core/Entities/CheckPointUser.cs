using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class CheckPointUser : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long PhaseId { get; set; }
        [ForeignKey(nameof(PhaseId))]
        public Phase Phase { get; set; }
        public long? ReviewerId { get; set; }
        [ForeignKey(nameof(ReviewerId))]
        public User Reviewer { get; set; }
        public long? UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public CheckPointUserType Type { get; set; }
        public int? Score { get; set; }
        public string Note { get; set; }
        public CheckPointUserStatus Status { get; set; }
        public bool IsPublic { get; set; }
    }
}
