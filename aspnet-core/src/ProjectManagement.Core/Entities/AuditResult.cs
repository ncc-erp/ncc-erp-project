using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class AuditResult : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(AuditSessionId))]
        public AuditSession AuditSession { get; set; }
        public long AuditSessionId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }

        [ForeignKey(nameof(PMId))]
        public User PM { get; set; }
        public long PMId { get; set; }
        public AuditResultStatus Status { get; set; }
    }
}
