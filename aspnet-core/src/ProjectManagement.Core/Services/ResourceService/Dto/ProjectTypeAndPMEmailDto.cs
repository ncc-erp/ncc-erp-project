using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ProjectTypeAndPMEmailDto
    {
        public string PMEmail{ get; set; }
        public ProjectType ProjectType { get; set; }
    }
}
