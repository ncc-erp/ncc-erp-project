using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Users.Dto
{
    public class ProjectHistoryDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public DateTime StartTime { get; set; }
        public byte allowcatePercentage { get; set; }
        public ProjectUserStatus Status { get; set; }
    }
}
