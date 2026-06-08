using NccCore.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Manager.OffboardUserManager.Dto
{
    public class InputGetAllOffboardHistoryDto : GridParam
    {
        public long? ProjectId { get; set; }
        public OffboardStatus? OffboardStatus { get; set; }
    }
}

