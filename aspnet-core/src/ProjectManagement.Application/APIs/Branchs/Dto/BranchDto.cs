using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.Branchs.Dto
{
    [AutoMap(typeof(Branch))]
    public class BranchDto : Entity<long>
    {
        [Required]
        [ApplySearchAttribute]
        public string Name { get; set; }
        [Required]
        [ApplySearchAttribute]
        public string DisplayName { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        [ApplySearchAttribute]
        public string Code { get; set; }
    }
}
