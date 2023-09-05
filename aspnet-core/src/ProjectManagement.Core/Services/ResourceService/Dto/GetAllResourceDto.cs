using NccCore.Anotations;
using ProjectManagement.Services.ResourceManager.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class GetAllResourceDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        [ApplySearchAttribute]
        public string EmailAddress { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        [ApplySearchAttribute]
        public string NormalFullName { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType? UserType { get; set; }
        public UserLevel? UserLevel { get; set; }
        public Branch Branch { get; set; }
        public int Used { get; set; }
        public List<ProjectOfUserDto> PlanProjects { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }
        public int? StarRate { get; set; }
        public List<ProjectOfUserDto> WorkingProjects { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public long? BranchId { get; set; }
        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }
    }
}
