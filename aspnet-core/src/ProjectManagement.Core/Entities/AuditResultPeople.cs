using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class AuditResultPeople : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(AuditResultId))]
        public AuditResult AuditResult { get; set; }
        public long AuditResultId { get; set; }

        [ForeignKey(nameof(CheckListItemId))]
        public CheckListItem CheckListItem { get; set; }
        public long CheckListItemId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long UserId { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }

        public bool IsPass { get; set; }

        [ForeignKey(nameof(CuratorId))]
        public User Curator { get; set; }
        public long? CuratorId { get; set; }
    }
}
