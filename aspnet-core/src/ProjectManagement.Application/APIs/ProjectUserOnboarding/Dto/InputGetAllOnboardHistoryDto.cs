using NccCore.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserOnboarding.Dto
{
    public class InputGetAllOnboardHistoryDto : GridParam
    {
        public long? ProjectId { get; set; }
        public ProjectUserOnboardingStatus? Status { get; set; }
        public long? PMId { get; set; }
    }
}
