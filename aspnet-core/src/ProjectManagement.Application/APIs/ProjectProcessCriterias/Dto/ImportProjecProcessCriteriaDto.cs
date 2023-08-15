using Microsoft.AspNetCore.Http;

namespace ProjectManagement.APIs.ProjectProcessCriterias.Dto
{
    public class ImportProjecProcessCriteriaDto
    {
        public IFormFile File { get; set; }
        public long ProjectId { get; set; }
    }
}