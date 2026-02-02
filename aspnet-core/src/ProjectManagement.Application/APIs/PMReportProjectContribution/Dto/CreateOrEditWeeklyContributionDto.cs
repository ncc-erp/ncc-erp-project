using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class CreateOrEditWeeklyContributionDto
    {
        public long? Id { get; set; }

        public long UserId { get; set; }

        public long ProjectId { get; set; }

        public long ProjectUserBillId { get; set; }

        public long PMReportId { get; set; }

        public byte Contribute { get; set; }
    }
}
