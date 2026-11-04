using System.Collections.Generic;

namespace ProjectManagement.APIs.ProjectKnowledgeBases.Dto
{
    public class KnowledgeBaseWeeklyReportDto
    {
        public string ReportName { get; set; }
        public List<KnowledgeBaseCriterionDto> Criteria { get; set; } = new List<KnowledgeBaseCriterionDto>();
    }
}
