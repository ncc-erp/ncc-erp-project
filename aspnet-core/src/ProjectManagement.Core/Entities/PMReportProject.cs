using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class PMReportProject : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long PMReportId { get; set; }
        [ForeignKey(nameof(PMReportId))]
        public PMReport PMReport { get; set; }
        public long ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public PMReportProjectStatus Status { get; set; }
        public DateTime? TimeSendReport { get; set; }
        public ProjectHealth ProjectHealth { get; set; }
        public bool NecessaryReview { get; set; }
        public long PMId { get; set; }
        [ForeignKey(nameof(PMId))]
        public User PM { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
        public string AutomationNote { get; set; }
        public bool Seen { get; set; }
        public PunishStatus IsPunish { get; set; }
        public int TotalNormalWorkingTime { get; set; }
        public int TotalOverTime { get; set; }
        public DateTime? LastReviewDate { get; set; }
    }
}