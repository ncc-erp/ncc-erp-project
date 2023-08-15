using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    [AutoMapTo(typeof(ResourceRequest))]
    public class UpdateResourceRequestDto : EntityDto<long>
    {
        public string Name { get; set; }
        public long ProjectId { get; set; }
        public DateTime TimeNeed { get; set; }
        public UserLevel Level { get; set; }
        public Priority Priority { get; set; }
        public List<long> SkillIds { get; set; }
        public int Quantity { get; set; }
    }
}
