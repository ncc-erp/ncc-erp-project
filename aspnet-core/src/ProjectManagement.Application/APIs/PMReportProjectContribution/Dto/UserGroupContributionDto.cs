using System.Collections.Generic;

namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class UserGroupContributionDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string AvatarPath { get; set; }
        public string BranchDisplayName { get; set; }
        public string BranchColor { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }
        public float TotalHeadCount { get; set; }
        public List<ProjectUserContributionDto> Projects { get; set; }
    }

    public class ProjectUserContributionDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PMName { get; set; }
        public double TotalContribute { get; set; } 
        public List<ProjectBillDetailDto> BillDetails { get; set; }
    }

    public class ProjectBillDetailDto
    {
        public string AccountName { get; set; }
        public string BillRole { get; set; }
        public double Contribute { get; set; }
        public double HeadCount { get; set; }
    }
}
