using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Entities
{
    public class WeeklyContributionHistory : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public long UserId { get; set; }

        public long ProjectUserBillId { get; set; }

        [ForeignKey(nameof(ProjectUserBillId))]
        public ProjectUserBill ProjectUserBill { get; set; }

        public long PMReportId { get; set; }

        [ForeignKey(nameof(PMReportId))] 
        public PMReport PMReport { get; set; }

        public long ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public byte Contribute { get; set; }
    }
}
