using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceManager.Dto
{
    public class PMOfUserDto
    {
        public BaseUserInfo PM { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        
    }
}
