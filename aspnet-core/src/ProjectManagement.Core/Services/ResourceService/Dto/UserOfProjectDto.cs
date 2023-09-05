using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceManager.Dto
{    
    public class UserOfProjectDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }

        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public Branch Branch { get; set; }
        public bool IsAvtive { get; set; }
        public byte AllocatePercentage { get; set; }
        public DateTime StartTime { get; set; }
        public int? StarRate { get; set; }

        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }

        public ProjectUserStatus PUStatus { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }

        public long? PMReportId { get; set; }
        public bool IsPool { get; set; }
        public string Note { get; set; }

        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }

        public ProjectUserStatus WorkingStatus
        {
            get
            {
                if (PUStatus == ProjectUserStatus.Present && AllocatePercentage > 0)
                {
                    return ProjectUserStatus.Present;
                }
                if (PUStatus == ProjectUserStatus.Past || (PUStatus == ProjectUserStatus.Present && AllocatePercentage <= 0))
                {
                    return ProjectUserStatus.Past;
                }
                if (PUStatus == ProjectUserStatus.Future)
                {
                    return ProjectUserStatus.Future;
                }
                return ProjectUserStatus.Present;
            }

        }

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
    public class ProjectStatusUser
    {
        public ProjectUserStatus Status { get; set; }
        public List<UserOfProjectDto> UserOfProjectDtos { get; set; }
    }
}
