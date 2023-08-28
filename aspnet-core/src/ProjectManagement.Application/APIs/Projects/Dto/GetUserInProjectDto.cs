using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class GetUserInProjectDto
    {
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<ProjectUser> ProjectUsers { get; set; }

    }
}
