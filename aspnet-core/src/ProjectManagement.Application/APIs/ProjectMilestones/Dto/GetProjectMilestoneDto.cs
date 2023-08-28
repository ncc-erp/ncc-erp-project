
using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.APIs.ProjectMilestones.Dto
{
    public class GetProjectMilestoneDto: Entity<long>
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        public string Flag { get; set; }
        public string Status { get; set; }
        public DateTime? UATTimeStart { get; set; }
        public DateTime? UATTimeEnd { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
    }
}
