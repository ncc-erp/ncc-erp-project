using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class ProjectInfoDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectType ProjectType { get; set; }
        public ProjectStatus ProjectStatus { get; set; }

        public string PrjectTypeName
        {
            get
            {
                return CommonUtil.ProjectTypeName(ProjectType);
            }

        }

        public string PrjectStatusName
        {
            get
            {
                return CommonUtil.ProjectStatusName(ProjectStatus);
            }

        }
    }
}
