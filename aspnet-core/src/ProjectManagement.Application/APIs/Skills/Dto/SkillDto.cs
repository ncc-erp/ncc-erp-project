using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Skills.Dto
{
    [AutoMapTo(typeof(Skill))]
    public class SkillDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
    }
}
