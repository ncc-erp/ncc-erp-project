using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.APIs.CheckListItems.Dto
{
    [AutoMapTo(typeof(CheckListItem))]
    public class CheckListItemDto : Entity<long>
    {
        [MaxLength(255)]
        public string Name { get; set; }
        public string Code { get; set; }
        public long CategoryId { get; set; }
        [MaxLength(10000)]
        public string Description { get; set; }
        [MaxLength(255)]
        public string AuditTarget { get; set; }
        [MaxLength(255)]
        public string PersonInCharge { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
    }
}
