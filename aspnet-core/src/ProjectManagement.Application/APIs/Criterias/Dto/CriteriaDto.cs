using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Criterias.Dto
{
    [AutoMapTo(typeof(Criteria))]
    public class CriteriaDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public int Weight { get; set; }
        public string Note { get; set; }
        public long CriteriaCategoryId { get; set; }
    }
}
