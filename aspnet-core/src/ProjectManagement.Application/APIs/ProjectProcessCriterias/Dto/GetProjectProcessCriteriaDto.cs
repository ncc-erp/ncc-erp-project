using Abp.Domain.Entities;
using ProjectManagement.GeneralModels;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessCriterias.Dto
{
    public class GetAllProjectProcessCriteriaDto : Entity<long>
    {
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public string PMName { get; set; }
        public long ProjectId { get; set; }
        public string ProjectType { get; set; }
        public string ClientName { get; set; }
        public long ProcessCriteriaId { get; set; }
        public List<long> ListProcessCriteriaIds { get; set; }
    }

    public class InputToGetAllDto
    {
        public long? ProjectId { get; set; }
        public long? ProcessCriteriaId { get; set; }

        public bool GetAll()
        {
            return (ProjectId == null || ProjectId == 0) && (ProcessCriteriaId == null || ProcessCriteriaId == 0);
        }
    }

    public class InputToGetDetail
    {
        public long ProjectId { get; set; }
        public string SearchText { get; set; }
        public Applicable Applicable { get; set; }

        public bool GetAll()
        {
            return string.IsNullOrEmpty(SearchText) && Applicable < Applicable.Standard;
        }
    }

    public class ProcessCriteriaOfProjectDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActive { get; set; }
        public long? ParentId { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
        public long Id { get; set; }
    }

    public class CreateProjectProcessCriteriaDto
    {
        public List<long> ProjectIds { get; set; }
    }

    public class UpdateProjectProcessCriteriaDto
    {
        public long ProjectId { get; set; }
        public List<long> ProcessCriteriaIds { get; set; }
    }

    public class UpdateNoteApplicableDto : Entity<long>
    {
        public string Note { get; set; }
        public Applicable Applicable { get; set; }
    }

    public class OneCriteriaToMutilProjectDto
    {
        public long ProcessCriteriaId { get; set; }
    }

    public class GetProjectProcessCriteriaDto
    {
        public long ProjectId { get; set; }
        public long ProcessCriteriaId { get; set; }
    }

    public class DeleteCriteriaDto
    {
        public long ProjectId { get; set; }
        public List<long> ProcessCriteriaIds { get; set; }
    }

    public class InputToGetAllProjectToAddDto
    {
        public string SearchText { get; set; }
        public ProjectType? ProjectType { get; set; }

        public bool GetAll()
        {
            return string.IsNullOrEmpty(SearchText) && ProjectType == null;
        }
    }

    public class GetAllProjectToAddDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public ProjectType Type { get; set; }
        public string PMName { get; set; }
        public string ClientName { get; set; }
    }

    public class GetProjectProcessCriteriaTreeDto : Entity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActive { get; set; }
        public string GuidLine { get; set; }
        public long? ParentId { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
        public string Note { get; set; }
        public long ProjectProcessCriteriaId { get; set; }
        public Applicable Applicable { get; internal set; }
    }

    public class TreeProjectProcessCriteriaDto
    {
        public long Id => 0;
        public string Code => "Root";
        public string Name => "Root";
        public bool IsLeaf => false;
        public IEnumerable<TreeItem<GetProjectProcessCriteriaTreeDto>> Childrens { get; set; }
    }
}