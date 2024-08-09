using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class ResourceRequestSetDoneDto
    {
        public long RequestId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime? BillStartTime { get; set; }
        public ProjectUserRole ProjectRole { get; set; }

    }
}
