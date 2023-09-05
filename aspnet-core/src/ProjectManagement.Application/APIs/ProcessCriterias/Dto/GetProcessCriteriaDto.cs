using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using ProjectManagement.GeneralModels;
using System.Collections.Generic;

namespace ProjectManagement.APIs.ProcessCriterias.Dto
{
    public class GetProcessCriteriaDto : Entity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActive { get; set; }
        public string GuidLine { get; set; }
        public string QAExample { get; set; }
        public long? ParentId { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
    }

    public class GetForDropDownDto : Entity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public bool IsActive { get; set; }
        public string GuidLine { get; set; }
        public string QAExample { get; set; }
        public long? ParentId { get; set; }
        public int Level { get; set; }
        public bool IsLeaf { get; set; }
        public int MaxValueOfListCode { get; set; }
    }

    public class InputToGetProcessCriteriaDto
    {
        public bool? IsActive { get; set; }
        public bool? IsApplicable { get; set; }
        public bool? IsLeaf { get; set; }
        public string SearchText { get; set; }

        public bool IsGetAll()
        {
            return !IsActive.HasValue && !IsApplicable.HasValue && string.IsNullOrEmpty(SearchText) &&!IsLeaf.HasValue;
        }
    }

    [AutoMap(typeof(ProcessCriteria))]
    public class CreateProcessCriteriaDto : Entity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public string GuidLine { get; set; }
        public string QAExample { get; set; }
        public long? ParentId { get; set; }
    }

    [AutoMap(typeof(ProcessCriteria))]
    public class UpdateProcessCriteriaDto : Entity<long>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsApplicable { get; set; }
        public string GuidLine { get; set; }
        public string QAExample { get; set; }
    }

    public class TreeCriteriaDto
    {
        public long Id => 0;
        public string Code => "Root";
        public string Name => "Root";
        public bool IsLeaf => false;
        public IEnumerable<TreeItem<GetProcessCriteriaDto>> Childrens { get; set; }
    }
}