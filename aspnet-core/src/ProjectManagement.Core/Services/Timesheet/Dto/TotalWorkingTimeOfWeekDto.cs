using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Timesheet.Dto
{
    public class TotalWorkingTimeOfWeekDto
    {
        public int NormalWorkingTime { get; set; }
        public int OverTime { get; set; }
        public string ProjectCode { get; set; }
    }
}
