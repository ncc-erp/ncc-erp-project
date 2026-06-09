using NccCore.Paging;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;

namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class ContributionAverageInputDto : GridParam
    {
        public List<long> BranchIds { get; set; }
        public long? ProjectId { get; set; }
        public List<ProjectEnum.UserType> UserTypes { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }

    public class ContributionAverageUserDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string AvatarPath { get; set; }
        public string BranchDisplayName { get; set; }
        public string BranchColor { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }
        public double AverageContribution { get; set; }
        public List<ContributionAverageProjectDto> Projects { get; set; }
    }

    public class ContributionAverageProjectDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PMName { get; set; }
        public double TotalContribute { get; set; }
        public List<ContributionAverageDetailDto> BillDetails { get; set; }
    }

    public class ContributionAverageDetailDto
    {
        public long PMReportId { get; set; }
        public string PMReportName { get; set; }
        public long ProjectUserBillId { get; set; }
        public string AccountName { get; set; }
        public string BillRole { get; set; }
        public double Contribute { get; set; }
        public double HeadCount { get; set; }
    }

    public class ContributionAverageResultDto
    {
        public List<ContributionAverageUserDto> Items { get; set; }
        public int TotalCount { get; set; }
        public int WeeklyReportInRange { get; set; }
    }
}