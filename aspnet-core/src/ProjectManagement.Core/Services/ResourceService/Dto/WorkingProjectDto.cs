using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class WorkingProjectDto
    {
        public long projectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectUserRole ProjectRole { get; set; }        
        public DateTime StartTime { get; set; }
        public bool IsPool { get; set; }
        public string WorkType
        {
            get
            {
                return CommonUtil.ProjectUserWorkType(this.IsPool);
            }
        }

    }
}
