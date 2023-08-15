using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProcessCriteria : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActive { get; set; }
        public string GuidLine { get; set; }
        public string QAExample { get; set; }
        public long? ParentId { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }

    }
}
