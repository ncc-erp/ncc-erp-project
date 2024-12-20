using Abp.AutoMapper;
using Abp.Domain.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Entities;

namespace ProjectManagement.Services.Backup.Dto
{
    [AutoMapTo(typeof(MonthlyUserContribution))]
    public class MonthlyUserContributionDto : Entity<long>
    {
        public long UserId { get; set; }
        public UserLevel UserLevel { get; set; }
        public UserType UserType { get; set; }
        public long? BranchId { get; set; }
        public byte Contribute { get; set; }
        public long ProjectId { get; set; }
        public byte MonthTime { get; set; }
        public int YearTime { get; set; }
    }
}