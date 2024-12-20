using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class MonthlyUserContribution : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        
        public long? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }
        
        public long ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        
        public byte Contribute { get; set; }
        public UserLevel UserLevel { get; set; }
        public UserType UserType { get; set; }
        public byte MonthTime { get; set; }
        public int YearTime { get; set; }
    }
}