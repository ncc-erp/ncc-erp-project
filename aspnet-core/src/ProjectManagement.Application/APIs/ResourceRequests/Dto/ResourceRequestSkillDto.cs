using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    [AutoMapTo(typeof(ResourceRequestSkill))]
    public class ResourceRequestSkillDto : EntityDto<long>
    {
        public long ResourceRequestId { get; set; }
        public long SkillId { get; set; }
        public long Quantity { get; set; }
    }
}
