using NccCore.Anotations;
using NccCore.Uitls;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class GetAllPoolResourceDto
    {
        public long UserId { get; set; }
        [ApplySearchAttribute]
        public string EmailAddress { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }        
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType? UserType { get; set; }
        public UserLevel? UserLevel { get; set; }
        public Branch Branch { get; set; }
        public List<ProjectOfUserDto> PlannedProjects { get; set; }
        public List<ProjectOfUserDto> PoolProjects { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }
        public int? StarRate { get; set; }
        
        public string PoolNote { get; set; }
        public bool IsPool { get; set; }
        public DateTime LastReleaseDate { get; set; }
        public DateTime UserCreationTime { get; set; }

        public int TotalFreeDay
        {
            get
            {
                if (this.LastReleaseDate > this.UserCreationTime)
                {
                    return (DateTimeUtils.GetNow().Date - this.LastReleaseDate.Date).Days;
                }
                return (DateTimeUtils.GetNow().Date - this.UserCreationTime.Date).Days;

            }
        }

        public DateTime DateStartPool
        {
            get
            {
                return this.LastReleaseDate > this.UserCreationTime ? this.LastReleaseDate : this.UserCreationTime;
            }
        }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public long? BranchId { get; set; }

        public long? PositionId { get; set; }
        public string PositionColor { get; set; }

        public string PositionName { get; set; }
    }
}
