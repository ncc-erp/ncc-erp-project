using NccCore.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class InputGetAllRequestResourceDto : GridParam
    {
        public List<long> SkillIds { get; set; }
        public bool IsAndCondition { get; set; }
        public bool IsTraining { get; set; }
        public IDictionary<string, SortDirection> SortParams { get; set; }
        public List<string> FilterRequestCode { get; set; }
        public List<ResourceRequestStatus> FilterRequestStatus { get; set; }
    }
}
