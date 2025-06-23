using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Public.Dto
{
    public class LateReportPMDto
    {
        public long PMId { get; set; }
        public long ProjectId { get; set; }
        public DateTime? TimeSendReport { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
    }
}
