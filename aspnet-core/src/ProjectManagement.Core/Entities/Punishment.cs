using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ProjectManagement.Entities
{
    public class Punishment : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Note { get; set; }
    }
}
