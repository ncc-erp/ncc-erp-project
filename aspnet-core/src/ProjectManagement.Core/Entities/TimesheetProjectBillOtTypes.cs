using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Text;

namespace ProjectManagement.Entities
{
    public class TimesheetProjectBillOtTypes : FullAuditedEntity<long>
    {

        [ForeignKey(nameof(ProjectOtTypeId))]
        public ProjectOtType ProjectOtType { get; set; }
        public long ProjectOtTypeId { get; set; }

        public long TimesheetProjectBillId { get; set; }
        public decimal Hours { get; set; }
        
        [ForeignKey(nameof(TimesheetProjectBillId))]
        public TimesheetProjectBill TimesheetProjectBill { get; set; }
 
    }
}
