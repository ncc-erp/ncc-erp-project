using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Timesheet.Dto
{
    public class InputRetroReviewInternHistoriesDto
    {
        public List<string> Emails { get; set; }
        public int MaxCountHistory { get; set; } = 12;
    }
}
