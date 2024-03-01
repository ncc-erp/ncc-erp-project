using Abp.Application.Services.Dto;
using NccCore.Anotations;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class GetAllUserDto : EntityDto<long>
    {

        [ApplySearchAttribute]
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public ProjectManagement.Constants.Enum.ProjectEnum.Branch Branch { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }
        public bool IsActive { get; set; }

        [ApplySearchAttribute]
        public string FullName { get; set; }

        public DateTime CreationTime { get; set; }

        public string UserName { get; set; }

        public ICollection<WorkingProjectDto> WorkingProjects { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }

        public long? PositionId { get; set; }

        public string PositionColor { get; set; }

        public string PositionName { get; set; }
    }

    public class GetUserInfo : EntityDto<long>
    {
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public ProjectManagement.Constants.Enum.ProjectEnum.Branch Branch { get; set; }
        public bool IsActive { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }

        public long? PositionId { get; set; }

        public string PositionColor { get; set; }

        public string PositionName { get; set; }
    }
}