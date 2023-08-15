using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Entities
{
    public class CheckPointUserDetail : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long CheckPointUserId { get; set; }
        [ForeignKey(nameof(CheckPointUserId))]
        public CheckPointUser CheckPointUser { get; set; }
        public long CriteriaId { get; set; }
        [ForeignKey(nameof(CriteriaId))]
        public Criteria Criteria { get; set; }
        public int? Score { get; set; }
        public string Note { get; set; }
    }
}
