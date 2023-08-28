using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ResourceRequestSkill : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long ResourceRequestId { get; set; }
        [ForeignKey(nameof(ResourceRequestId))]
        public ResourceRequest ResourceRequest { get; set; }
        public long Quantity { get; set; }
        public long SkillId { get; set; }
        [ForeignKey(nameof(SkillId))]
        public Skill Skill { get; set; }

    }
}
