using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ProjectUserOnboardStatusDto
    {
        public long UserId { get; set; }
        public bool IsOnboarded { get; set; } = false;
    }
}
