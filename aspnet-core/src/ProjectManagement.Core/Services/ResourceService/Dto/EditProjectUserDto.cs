using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class EditProjectUserDto
    {
        public long ProjectUserId { get; set; }
        public long ProjectId { get; set; }
        public DateTime StartTime { get; set; }
        public byte AllocatePercentage { get; set; }
        public string Note { get; set; }
        public bool IsPool { get; set; }
        public ProjectUserRole ProjectRole { get; set; }

    }
}
