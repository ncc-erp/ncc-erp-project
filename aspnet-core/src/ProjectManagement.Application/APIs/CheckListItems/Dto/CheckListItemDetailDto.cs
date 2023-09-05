
using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckListItems.Dto
{
    [AutoMapTo(typeof(CheckListItem))]
    public class CheckListItemDetailDto : Entity<long>
    {
        [MaxLength(255)]
        [ApplySearchAttribute]
        public string Name { get; set; }
        [ApplySearchAttribute]
        public string Code { get; set; }
        public long CategoryId { get; set; }
        [MaxLength(1000)]
        [ApplySearchAttribute]
        public string CategoryName { get; set; }
        [MaxLength(10000)]
        [ApplySearch]
        public string Description { get; set; }
        [MaxLength(255)]
        [ApplySearch]
        public string AuditTarget { get; set; }
        [MaxLength(255)]
        [ApplySearch]
        public string PersonInCharge { get; set; }
        [MaxLength(10000)]
        [ApplySearch]
        public string Note { get; set; }
        [ApplySearch]
        public ProjectType ProjectType { get; set; }
        public List<ProjectType> mandatorys { get; set; }
    }
}
