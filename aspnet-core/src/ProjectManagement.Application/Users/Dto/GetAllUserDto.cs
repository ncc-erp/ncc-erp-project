using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Services.ResourceService.Dto;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;
using ProjectManagement.Constants.Enum;

namespace ProjectManagement.Users.Dto
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

        public string[] RoleNames { get; set; }

        public List<WorkingProjectDto> WorkingProjects { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }

        public long? PositionId { get; set; }

        public string PositionColor { get; set; }

        public string PositionName { get; set; }

    }
}
