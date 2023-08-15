
using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectMilestones.Dto
{
    [AutoMapTo(typeof(ProjectMilestone))]
    public class ProjectMilestoneDto:Entity<long>
    {
        public long ProjectId { get; set; }
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
