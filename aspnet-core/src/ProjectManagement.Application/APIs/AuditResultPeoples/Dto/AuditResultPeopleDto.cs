using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.APIs.AuditResultPeoples.Dto
{
    [AutoMapTo(typeof(AuditResultPeople))]
    public class AuditResultPeopleDto: Entity<long>
    {
        public long AuditResultId { get; set; }
        public long CheckListItemId { get; set; }
        public long UserId { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
        public long? CuratorId { get; set; }
        public bool IsPass { get; set; }
    }
}
