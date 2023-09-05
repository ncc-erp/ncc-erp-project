using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Entities
{
    public class Branch : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Color { get; set; }
        public string Code { get; set; }
    }
}
