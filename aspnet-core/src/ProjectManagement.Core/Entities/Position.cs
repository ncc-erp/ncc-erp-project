using Abp.Domain.Entities.Auditing;

namespace ProjectManagement.Entities
{
    public class Position : FullAuditedEntity<long>
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Code { get; set; }

        public string Color { get; set; }
    }
}