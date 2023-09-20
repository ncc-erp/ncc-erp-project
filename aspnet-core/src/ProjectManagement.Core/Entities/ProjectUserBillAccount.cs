using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProjectUserBillAccount : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }

        public long UserBillAccountId { get; set; }
    }
}
