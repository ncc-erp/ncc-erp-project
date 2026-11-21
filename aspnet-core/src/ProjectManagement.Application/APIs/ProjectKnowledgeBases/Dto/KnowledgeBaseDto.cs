using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.APIs.ProjectKnowledgeBases.Dto
{
    public class KnowledgeBaseProjectInput
    {
        public long ProjectId { get; set; }
    }

    public class UpdateKnowledgeBaseInput : KnowledgeBaseProjectInput
    {
        [Required(AllowEmptyStrings = true)]
        [Abp.Auditing.DisableAuditing]
        public string Content { get; set; }
    }

    public class SavedKnowledgeBaseDto
    {
        public string ProjectId { get; set; }
        public string Content { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class KnowledgeBaseDto : SavedKnowledgeBaseDto
    {
        public List<KnowledgeBaseReferenceFileDto> ReferenceFiles { get; set; } = new List<KnowledgeBaseReferenceFileDto>();
    }

    public class GeneratedKnowledgeBaseDto
    {
        public string Content { get; set; }
    }

    public class UpdateKnowledgeBaseReferencesInput : KnowledgeBaseProjectInput
    {
        [Required]
        public List<long> ProjectFileIds { get; set; }
    }

    public class KnowledgeBaseReferenceFileDto
    {
        public string ProjectFileId { get; set; }
        public string Name { get; set; }
    }

    public class KnowledgeBaseFileOptionDto : KnowledgeBaseReferenceFileDto
    {
        public bool IsSelected { get; set; }
    }
}
