using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Constants.Enum;

namespace ProjectManagement.Entities
{
    public class BillUserSkill : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long SkillId { get; set; }
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; }
        public ProjectEnum.SkillRank SkillRank { get; set; }
        public long BillId { get; set; }
        [ForeignKey(nameof(BillId))]
        public ProjectUserBill Bill { get; set; }
        public string Note { get; set; }
    }
}