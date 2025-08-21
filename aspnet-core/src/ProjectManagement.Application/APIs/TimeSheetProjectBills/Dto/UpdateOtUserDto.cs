using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class UpdateOtUserDto
    {
        public int OtId { get; set; }
        public int TimesheetProjectBillId { get; set; }
        public string OtType { get; set; }
        public decimal OtHours { get; set; }
        public float Multiplier { get; set; }
    }
}
