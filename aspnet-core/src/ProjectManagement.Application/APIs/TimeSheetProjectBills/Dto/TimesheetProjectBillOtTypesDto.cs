using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class TimesheetProjectBillOtTypesDto
    {
        public long Id { get; set; }
        public long TimesheetProjectBillId { get; set; }
        public string OtType { get; set; }
        public decimal OtHours { get; set; }
        public float Multiplier { get; set; }
    }
}
