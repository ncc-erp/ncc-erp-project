using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceManager.Dto
{
    public class ResourceHisotryDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public DateTime StartTime { get; set; }
        public byte allowcatePercentage { get; set; }
        public ProjectUserStatus Status { get; set; }
    }
}
