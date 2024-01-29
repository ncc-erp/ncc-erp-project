using static ProjectManagement.Constants.Enum.ProjectEnum;
using System;

namespace ProjectManagement.Services.ResourceManager
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