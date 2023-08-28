using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class Project : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(255)]
        public string Code { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectStatus Status { get; set; }
        public long? ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public Client Client { get; set; }
        public long? CurrencyId { get; set; }
        [ForeignKey(nameof(CurrencyId))]
        public Currency Currency { get; set; }
        public bool? IsCharge { get; set; }
        public ChargeType? ChargeType { get; set; }
        public long PMId { get; set; }
        [ForeignKey(nameof(PMId))]
        public User PM { get; set; }
        public string BriefDescription { get; set; }
        public string DetailDescription { get; set; }
        public string TechnologyUsed { get; set; }
        public string TechnicalProblems { get; set; }
        public string OtherProblems { get; set; }
        public string NewKnowledge { get; set; }
        public string Evaluation { get; set; }
        public bool RequireTimesheetFile { get; set; }
        public long LastInvoiceNumber { get; set; }
        public float Discount { get; set; }
        public long? ParentInvoiceId { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public bool IsRequiredWeeklyReport { get; set; }
    }
}