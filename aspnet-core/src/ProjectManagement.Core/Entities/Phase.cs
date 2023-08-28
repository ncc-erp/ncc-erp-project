using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class Phase: FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public long ParentId { get; set; }
        public PhaseType Type { get; set; }
        public PhaseStatus Status { get; set; }
        public bool IsCriteria { get; set; }
        public int Index { get; set; }
    }
}
