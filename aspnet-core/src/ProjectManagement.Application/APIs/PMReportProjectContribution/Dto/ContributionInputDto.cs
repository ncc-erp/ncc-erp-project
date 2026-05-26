using NccCore.Paging;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class ContributionInputDto : GridParam
    {
        public List<long> BranchIds { get; set; }
        public long? ProjectId { get; set; }
        public List<UserType> UserTypes { get; set; }
    }
}
