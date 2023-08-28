using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.HRMv2.Dto
{
    public class InputUpdateUserIntoProjectDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; } 
        public string Note { get; set; }
        public long ActiveReportId { get; set; }
        public DateTime StartTime { get; set; }

        public bool IsPool { get; set; }
    }
}
