using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.PmBot.Dto
{
    public class SyncMeetingReportRequestDto
    {
        public long ProjectId { get; set; }
    }

    public class SyncMeetingReportResponseDto
    {
        public bool Success { get; set; }
    }
}

