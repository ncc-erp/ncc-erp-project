using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectKnowledgeBases.Dto
{
    public class KnowledgeBaseGeneralDto
    {
        public ProjectType ProjectType { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string ProjectManager { get; set; }
        public string Customer { get; set; }
        public string Currency { get; set; }
        public ChargeType? ChargeType { get; set; }
    }
}
