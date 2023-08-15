using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUsers.Dto
{
    public class UpdateProjectUserDto : EntityDto
    {
        public ProjectUserRole ProjectRole { get; set; }
        public bool IsPool { get; set; }
        public DateTime StartTime { get; set; }
    }
}
