using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateResouceRequestCVSendCVDate
    {
        public long ResourceRequestCVId { get; set; }
        public DateTime? SendCVDate { get; set; }
    }

    public class UpdateResouceRequestCVInterviewTime
    {
        public long ResourceRequestCVId { get; set; }
        public DateTime? InterviewDate { get; set; }
    }
}
