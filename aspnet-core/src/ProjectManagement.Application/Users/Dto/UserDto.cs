using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceService.Dto;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserDto : EntityDto<long>
    {
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        [ApplySearchAttribute]
        public string EmailAddress { get; set; }
        public string UserCode { set; get; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public ProjectManagement.Constants.Enum.ProjectEnum.Branch Branch { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }
        public bool IsActive { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        [ApplySearchAttribute]
        public string FullNameNormal { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public DateTime CreationTime { get; set; }

        public string[] RoleNames { get; set; }

        public string PoolNote { get; set; }

        public List<WorkingProjectDto> WorkingProjects { get; set; }
        public List<WorkingProjectDto> ProjectHistory { get; set; }
        public long? BranchId { get; set; }

        public long? PositionId { get; set; }
    }
}
