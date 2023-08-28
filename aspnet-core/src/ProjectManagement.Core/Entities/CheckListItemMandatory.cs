using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class CheckListItemMandatory : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(CheckListItemId))]
        public CheckListItem CheckListItem { get; set; }
        public long CheckListItemId { get; set; }

        public ProjectType ProjectType { get; set; }
    }
}
