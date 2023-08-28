using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ProjectUserPlans
    {
        public long CreatorUserId { get; set; }
        public long ProjectUserId { get; set; }
        public string ProjectName { get; set; }
        public ProjectType ProjectType { get; set; }

        public DateTime StartTime { get; set; }
        public int AllocatePercentage { get; set; }
    }
}
