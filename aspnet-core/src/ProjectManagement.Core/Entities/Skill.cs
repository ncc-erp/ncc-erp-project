using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.Entities
{
    public class Skill : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
    }
}
