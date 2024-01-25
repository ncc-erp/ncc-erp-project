using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateTempProjectForUserDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsPool { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
    }
}
