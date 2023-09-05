using NccCore.Anotations;
using ProjectManagement.APIs.Skills.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Users.Dto;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class AvailableResourceDto
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
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public Branch Branch { get; set; }
        public List<WorkingProjectDto> WorkingProjects { get; set; }
        public int Used { get; set; }
        public List<ProjectUserPlan> ProjectUserPlans { get; set; }
        public List<SkillDto> ListSkills { get; set; }
        public List<long> ListSkillIds { get; set; }
        public int? StarRate { get; set; }
        public int TotalFreeDay { get; set; }

        public string PoolNote { get; set; }

        public DateTime  DateStartPool { get; set; }
    }

    public class ProjectUserPlan
    {
        public long CreatorUserId { get; set; }
        public long ProjectUserId { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartTime { get; set; }
        public int AllocatePercentage { get; set; }
    }
    public class ProjectBaseDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}
