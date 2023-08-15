using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class UpdateTimesheetPMDto
    {
        public long Id { get; set; }
        public string AccountName { get; set; }
        public float WorkingTime { get; set; }
        public bool IsActive { get; set; }
        public string Note { get; set; }
    }
}
