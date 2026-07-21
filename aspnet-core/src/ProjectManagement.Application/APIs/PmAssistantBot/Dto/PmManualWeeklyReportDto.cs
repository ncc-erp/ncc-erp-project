using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.PmAssistantBot.Dto
{ 
    public class PmManualWeeklyReportInput
    {
        public long ProjectId { get; set; }

        public DateTime WeekStart { get; set; }

        public DateTime WeekEnd { get; set; }
    }
    public class PmManualWeeklyReportDto
{
        public string Name { get; set; }

        public string Note { get; set; }
    }
}