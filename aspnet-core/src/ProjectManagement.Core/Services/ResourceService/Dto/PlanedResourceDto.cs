using Abp.Application.Services.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceManager.Dto
{
    public class PlanedResourceDto 
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string FullName { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public byte AllocatePercentage { get; set; }
        public DateTime StartTime { get; set; }
        public long PMReportId { get; set; }
        public string PMReportName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public string Note { get; set; }
        public int? StarRate { get; set; }
        public UserLevel UserLevel { get; set; }
        public bool IsPool { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }

        public string WorkType
        {
            get
            {
                return CommonUtil.ProjectUserWorkType(this.IsPool);
            }
        }
        public string ProjectRoleName
        {
            get
            {
                return CommonUtil.ToString(this.ProjectRole);
            }
        }
    }
}
