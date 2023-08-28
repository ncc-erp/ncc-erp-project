using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class CheckPointUserResult : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long PhaseId { get; set; }
        [ForeignKey(nameof(PhaseId))]
        public Phase Phase { get; set; }
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long PMId { get; set; }
        [ForeignKey(nameof(PMId))]
        public User PM { get; set; }
        public string UserNote { get; set; }
        public string PMNote { get; set; }
        public string FinalNote { get; set; }
        public UserLevel OldLevel { get; set; }
        public UserLevel NewLevel { get; set; }
        public int? PMScore { get; set; }
        public int? TeamScore { get; set; }
        public int? ClientScore { get; set; }
        public int? SelfScore { get; set; }
        public int? ExamScore { get; set; }
        public int? FinalScore { get; set; }
        public CheckPointUserResultStatus Status { get; set; }

    }
}
