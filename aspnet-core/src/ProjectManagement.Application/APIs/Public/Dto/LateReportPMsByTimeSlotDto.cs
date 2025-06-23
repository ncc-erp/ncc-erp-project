using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Public.Dto
{
    public class LateReportPMsByTimeSlotDto
    {
        public List<LateReportPMDto> From15To17 { get; set; }
        public List<LateReportPMDto> After17 { get; set; }
    }
}
