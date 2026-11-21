using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectKnowledgeBases.Dto
{
    public class KnowledgeBaseCriterionDto
    {
        public string Name { get; set; }
        public ProjectCriteriaResultStatus Status { get; set; }
        public string Note { get; set; }
    }
}
