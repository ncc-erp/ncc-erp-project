using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace ProjectManagement.Entities
{
    public class TimesheetProjectBillOtTypes : FullAuditedEntity<long>
    {

        public long TimesheetProjectBillId { get; set; }
        public decimal Hours { get; set; }
        public string OtType { get; set; }
        public float Multiplier { get; set; }
        [ForeignKey(nameof(TimesheetProjectBillId))]
        public TimesheetProjectBill TimesheetProjectBill { get; set; }
 
    }
}
