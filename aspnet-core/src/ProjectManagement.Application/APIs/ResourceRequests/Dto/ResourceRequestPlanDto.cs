using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class ResourceRequestPlanDto
    {
        public long ProjectUserId { get; set; }

        public long UserId { get; set; }

        public ProjectUserRole ProjectRole { get; set; }

        public DateTime StartTime { get; set; }

        public long? ResourceRequestId { get; set; }
    }
}
