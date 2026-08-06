using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.PmBot.Dto
{
    public class InactiveProjectWeeklyReportRequestDto
    {
        public long ProjectId { get; set; }
    }

    public class InactiveProjectWeeklyReportResponseDto
    {
        public bool Success { get; set; }
    }
}
