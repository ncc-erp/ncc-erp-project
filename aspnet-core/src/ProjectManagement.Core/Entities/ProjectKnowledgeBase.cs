using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Entities
{
    public class ProjectKnowledgeBase : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        public long ProjectId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        public string Content { get; set; }

        public string SourceFileIds { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public long? UpdatedByUserId { get; set; }
    }
}
