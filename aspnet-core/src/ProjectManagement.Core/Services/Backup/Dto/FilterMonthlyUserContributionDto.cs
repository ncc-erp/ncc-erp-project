using System.Collections.Generic;
using NccCore.Paging;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.Backup.Dto
{
    public class FilterMonthlyUserContributionDto : GridParam
    {
        public List<long> BranchIds { get; set; }
        public List<UserType> UserTypes { get; set; }
        public List<UserLevel> UserLevels { get; set; }
        public byte MonthTime { get; set; }
        public int YearTime { get; set; }
    }
}