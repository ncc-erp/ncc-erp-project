using NccCore.Paging;
using System.Collections.Generic;

namespace ProjectManagement.APIs.PMReportProjectContribution.Dto
{
    public class ContributionInputDto : GridParam
    {
        public List<long> BranchIds { get; set; }
    }
}
