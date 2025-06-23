using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Public.Dto
{
    public class LateReportPMsByTimeSlotDto
    {
        public List<LateReportPMDto> Between3To5PM { get; set; }
        public List<LateReportPMDto> After5PMOrMissing { get; set; }
    }
}
