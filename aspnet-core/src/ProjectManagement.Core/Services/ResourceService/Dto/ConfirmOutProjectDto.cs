using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ConfirmOutProjectDto
    {
        public long ProjectUserId { get; set; }
        public DateTime StartTime { get; set; }
    }
}
