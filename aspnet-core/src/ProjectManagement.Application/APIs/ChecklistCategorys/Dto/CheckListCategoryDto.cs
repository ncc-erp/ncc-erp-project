using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.APIs.ChecklistTitles.Dto
{
    [AutoMapTo(typeof(CheckListCategory))]
    public class CheckListCategoryDto : Entity<long>
    {
        [ApplySearchAttribute]
        [MaxLength(1000)]
        public string Name { get; set; }
    }
}
