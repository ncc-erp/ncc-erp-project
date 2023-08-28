using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class CheckPointUserResultTag: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
        public long CheckPointUserResultId { get; set; }
        [ForeignKey(nameof(CheckPointUserResultId))]
        public CheckPointUserResult CheckPointUserResult { get; set; }
    }
}
