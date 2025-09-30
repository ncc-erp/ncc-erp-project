using Abp.Domain.Entities.Auditing;

namespace ProjectManagement.Entities
{
    public class OnboardSteps : FullAuditedEntity<int>
    {
        public string Title { get; set; }   
        public string Content { get; set; }   
        public int Order { get; set; }
    }
}
