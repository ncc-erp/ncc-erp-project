using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProjectCheckList : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }

        [ForeignKey(nameof(CheckListItemId))]
        public CheckListItem CheckListItem { get; set; }
        public long CheckListItemId { get; set; }

        public bool IsActive { get; set; }
    }
}
