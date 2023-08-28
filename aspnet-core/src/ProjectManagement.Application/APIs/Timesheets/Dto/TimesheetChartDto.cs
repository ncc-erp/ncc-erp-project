using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Timesheets.Dto
{
    public class TimesheetChartDto
    {
        public List<string> Labels { get; set; }
        public List<double> ManMonths { get; set; }
        public List<double> ManDays { get; set; }
    }
}
