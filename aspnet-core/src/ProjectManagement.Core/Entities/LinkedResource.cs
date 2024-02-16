using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;
namespace ProjectManagement.Entities
{
    public class LinkedResource : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        //[ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }

        //[ForeignKey(nameof(UserBillAccountId))]
        public ProjectUserBillAccount ProjectUserBillAccount { get; set; }
        public long UserBillAccountId { get; set; }
    }
}
