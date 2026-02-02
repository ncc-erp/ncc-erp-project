using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class WeeklyContributionDto
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public long ProjectUserBillId { get; set; }
        public long PMReportId { get; set; }
        public string PMReportName { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public byte Contribute { get; set; } = 0;
    }
}
