using Abp.Application.Services.Dto;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class GetSkillDetailDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string SkillName { get; set; }
        [ApplySearchAttribute]
        public string ResourceRequestName { get; set; }
        public long ResourceRequestId { get; set; }
        public long SkillId { get; set; }
        public long Quantity { get; set; }
    }
}
