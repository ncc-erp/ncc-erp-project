using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.Services.Backup.Dto
{
    public class InfoMonthlyUserContributionDto
    {
        public UserInfo Employee { get; set; }
        public List<ProjectContributeDto> ProjectContributes {get; set;}
        public int TotalContribute => ProjectContributes.Sum(s => s.Contribute);
    }

    public class ProjectContributeDto
    {
        public long? Id { get; set; }
        public ProjectType ProjectType { get; set; }
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public byte Contribute { get; set; }
    }

    public class UserInfo : EntityDto<long>
    {
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public long? BranchId { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
    }
}