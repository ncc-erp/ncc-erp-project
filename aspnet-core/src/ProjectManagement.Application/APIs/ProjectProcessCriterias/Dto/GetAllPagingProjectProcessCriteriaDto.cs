using NccCore.Anotations;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessCriterias.Dto
{
    public class GetAllPagingProjectProcessCriteriaDto
    {
        public long ProjectId { get; set; }
        [ApplySearchAttribute]
        public string ProjectCode { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        [ApplySearchAttribute]
        public string PMName { get; set; }
        [ApplySearchAttribute]
        public string ClientCode { get; internal set; }
        [ApplySearchAttribute]
        public string ClientName { get; set; }
        public int CountCriteria { get; set; }
        public ProjectType ProjectType { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
    }
}