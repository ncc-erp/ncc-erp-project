using Abp.Application.Services.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceManager.Dto
{
    public class ProjectOfUserDto : EntityDto<long>
    {
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public string ProjectName { get; set; }
        public ProjectType ProjectType { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public string PmName { get; set; }
        public byte AllocatePercentage { get; set; }
        public DateTime StartTime { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public long PMReportId { get; set; }
        public ProjectUserStatus Status { get; set; }
        public bool IsPool { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectRoleName
        {
            get
            {
                return CommonUtil.ToString(this.ProjectRole);
            }
        }

        public string WorkType
        {
            get
            {
                return CommonUtil.ProjectUserWorkType(this.IsPool);
            }
        }

    }
}
