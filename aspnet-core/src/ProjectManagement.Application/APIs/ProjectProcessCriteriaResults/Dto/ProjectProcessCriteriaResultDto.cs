using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.APIs.ProcessCriterias.Dto;
using ProjectManagement.Entities;
using ProjectManagement.GeneralModels;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessCriteriaResults.Dto
{
    [AutoMap(typeof(ProjectProcessCriteriaResult))]
    public class GetProjectProcessCriteriaResultDto : Entity<long>
    {
        public long ProjectProcessResultId { get; set; }
        public long ProjectId { get; set; }
        public NCStatus Status { get; set; }
        public int Score { get; set; }
        public string Note { get; set; }
        public string TailoringNote { get; set; }
        public Applicable Applicable { get; set; }

        public virtual GetProcessCriteriaDto ProcessCriteria { get; set; }
    }

    public class TreeCriteriaResultDto
    {
        public int TotalScore { get; set; }
        public ProjectScoreKPIStatus Status { get; set; }
        public IEnumerable<TreeItem<GetProjectProcessCriteriaResultDto>> Childrens { get; set; }
    }

    public class InputToGetProjectProcessCriteriaResultDto
    {
        public long ProjectProcessResultId { get; set; }
        public long ProjectId { get; set; }
        public string SearchText { get; set; }
        public NCStatus? Status { get; set; }

        public bool IsGetAll()
        {
            return string.IsNullOrEmpty(SearchText) && !Status.HasValue;
        }
    }

    public class Tree<T>
    {
        public T Data { get; set; }
        public List<Tree<T>> Children { get; set; } = new List<Tree<T>>();
    }
}