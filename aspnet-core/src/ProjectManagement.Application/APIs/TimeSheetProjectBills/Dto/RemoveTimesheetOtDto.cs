using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class RemoveTimesheetOtDto
    {
        public int TimesheetProjectBillId { get; set; }
        public int OtId { get; set; }
    }
}
