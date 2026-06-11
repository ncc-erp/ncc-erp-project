using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectResources.Dto
{
    [AutoMapTo(typeof(ProjectResource))]
    public class ProjectResourceDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Note { get; set; }
        public long? ParentId { get; set; }
        public List<ProjectResourceDto> Childrens { get; set; } = new List<ProjectResourceDto>();
    }
}
