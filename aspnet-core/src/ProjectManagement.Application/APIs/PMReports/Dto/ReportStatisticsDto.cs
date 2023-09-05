using ProjectManagement.APIs.PMReportProjectIssues.Dto;
using ProjectManagement.APIs.ProjectUsers.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReports.Dto
{
    public class ReportStatisticsDto
    {
        public string Note { get; set; }
        public List<GetPMReportProjectIssueDto> Issues { get; set; }
        public List<ProjectUserStatistic> ResourceInTheWeek { get; set; }
        public List<ProjectUserStatistic> ResourceInTheFuture { get; set; }
    }

    public class ProjectUserStatistic
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(Avatar);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public string Email { get; set; }
        public int AllocatePercentage { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
    }
}
