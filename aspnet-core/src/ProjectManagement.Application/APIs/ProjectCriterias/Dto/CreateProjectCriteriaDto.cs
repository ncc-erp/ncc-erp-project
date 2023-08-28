using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectCriterias.Dto
{
    [AutoMapTo(typeof(ProjectCriteria))]
    public class CreateProjectCriteriaDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Guideline { get; set; }

        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}