using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class GetUserForTimesheetProjectBillDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
