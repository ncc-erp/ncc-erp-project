using static ProjectManagement.Constants.Enum.ProjectEnum;
using System;

namespace ProjectManagement.Services.ResourceManager
{
    public class UpdateTempProjectForUserDto
    {
        public long Id { get; set; }
        public long ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
    }
}