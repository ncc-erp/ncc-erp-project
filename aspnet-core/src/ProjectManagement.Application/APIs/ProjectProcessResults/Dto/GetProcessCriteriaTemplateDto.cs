using System.Linq.Expressions;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class GetProcessCriteriaTemplateDto 
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string GuidLine { get; set; }
        public bool IsActive { get; set; }
        public bool IsApplicable { get; set; }
        public string Name { get; set; }
        public bool IsLeaf { get; set; }
        public int Level { get; set; }
        public string PmNote { get; set; }
        public long? ParentId { get; set; }
        public string QAExample { get; set; }
    }
}