using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ProjectMilestone : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        public MilestoneFlag Flag { get; set; }
        public ProjectMilestoneStatus Status { get; set; }
        public DateTime? UATTimeStart { get; set; }
        public DateTime? UATTimeEnd { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
    }
}
