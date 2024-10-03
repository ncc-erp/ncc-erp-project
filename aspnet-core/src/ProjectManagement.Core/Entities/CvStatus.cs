using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Entities
{
    public class CvStatus : FullAuditedEntity<long>
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
