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
        public CvStatusTriggerAction? TriggerAction { get; set; }
    }

    public enum CvStatusTriggerAction : byte
    {
        CreateBillAccountIfEmpty = 0,    // Create Bill Account of Request if empty
        CreateOrUpdateBillAccount = 1     // Create/Update Bill Account of Request
    }
}
