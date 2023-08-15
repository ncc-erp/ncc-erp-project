using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;

namespace ProjectManagement.Services.ResourceRequestService.Dto
{
    public class ResourceRequestSkillDto : EntityDto<long>
    {
        public string Name { get; set; }
    }
}