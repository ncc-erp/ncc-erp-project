using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjects.Dto
{
    public class CurrentResourceDto
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string ProjectRole { get; set; }
        public int AllocatePercentage { get; set; }
        public int TotalPercent { get; set; }
    }
}
