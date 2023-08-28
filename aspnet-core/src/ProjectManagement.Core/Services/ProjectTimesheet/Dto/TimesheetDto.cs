using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ProjectTimesheet.Dto
{
    public class TimesheetDto
    {
        public long Id { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public bool IsActive { get; set; }
        public float? TotalWorkingDay { get; set; }
    }
}
