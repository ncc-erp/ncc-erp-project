using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.PMReportProjects.Dto
{
    public class CollectTimesheetDto
    {
        public int NormalWorkingTime { get; set; }
        public int OverTime { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string PMName { get; set; }
        public string Note { get; set; }
    }
}
