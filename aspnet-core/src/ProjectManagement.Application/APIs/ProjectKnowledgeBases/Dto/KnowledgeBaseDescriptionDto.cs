using System.Collections.Generic;

namespace ProjectManagement.APIs.ProjectKnowledgeBases.Dto
{
    public class KnowledgeBaseDescriptionDto
    {
        public string BriefDescription { get; set; }
        public string DetailDescription { get; set; }
        public string TechnologyUsed { get; set; }
        public List<string> Technologies { get; set; } = new List<string>();
        public string TechnicalProblems { get; set; }
        public string OtherProblems { get; set; }
        public string NewKnowledge { get; set; }
    }
}
