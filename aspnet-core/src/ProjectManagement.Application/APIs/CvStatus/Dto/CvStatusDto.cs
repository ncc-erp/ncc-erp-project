using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.CvStatus.Dto
{
    [AutoMap(typeof(Entities.CvStatus))]
    public class CvStatusDto : Entity<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
