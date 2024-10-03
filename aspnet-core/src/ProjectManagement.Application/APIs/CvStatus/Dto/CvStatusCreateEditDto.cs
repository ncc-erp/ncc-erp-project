using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.APIs.CvStatus.Dto
{
    [AutoMap(typeof(Entities.CvStatus))]
    public class CvStatusCreateEditDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
    }
}
