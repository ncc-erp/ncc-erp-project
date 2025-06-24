using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Public.Dto
{
    public class LateReportPMDto
    {
        public List<PMReportDto> Between3To5PM { get; set; }
        public List<PMReportDto> After5PMOrMissing { get; set; }
    }
}
