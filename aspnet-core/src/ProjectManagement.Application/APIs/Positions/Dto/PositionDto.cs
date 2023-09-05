using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.Positions.Dto
{
    [AutoMap(typeof(Position))]
    public class PositionDto:Entity<long>
    {
        [Required]
        [ApplySearchAttribute]
        public string Name { get; set; }
        [Required]
        [ApplySearchAttribute]
        public string ShortName { get; set; }
        [Required]
        [ApplySearchAttribute]
        public string Code { get; set; }

        public string Color { get; set; }
    }
}
