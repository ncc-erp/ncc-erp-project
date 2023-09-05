using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class CheckListItem : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public string Code { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public CheckListCategory CheckListCategory { get; set; }
        public long CategoryId { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        [MaxLength(255)]
        public string AuditTarget { get; set; }
        [MaxLength(255)]
        public string PersonInCharge { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }

    }
}
