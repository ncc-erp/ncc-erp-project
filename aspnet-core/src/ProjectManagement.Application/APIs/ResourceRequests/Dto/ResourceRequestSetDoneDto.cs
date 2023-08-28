using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class ResourceRequestSetDoneDto
    {
        public long RequestId { get; set; }

        public DateTime StartTime { get; set; }
    }
}
