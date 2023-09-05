using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectCriterias.Dto
{
    [AutoMapTo(typeof(ProjectCriteria))]
    public class GetProjectCriteriaDto : EntityDto<long>
    {
        public string Guideline { get; set; }

        [ApplySearchAttribute]
        public string Name { get; set; }

        public bool IsActive { get; set; }
        public bool IsExist { get; set; }
    }
}