using Abp.Domain.Entities;
using System;

namespace ProjectManagement.Entities
{
    public class ProjectKnowledgeBaseHistory : Entity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public long KnowledgeBaseId { get; set; }
        public ProjectKnowledgeBase KnowledgeBase { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
        public long? UpdateByUserId { get; set; }
    }
}
