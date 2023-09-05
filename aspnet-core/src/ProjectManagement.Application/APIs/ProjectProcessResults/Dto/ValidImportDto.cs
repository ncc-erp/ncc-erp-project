using ProjectManagement.Entities;
using System.Collections.Generic;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class ValidImportDto
    {
        public List<ProjectProcessCriteriaResult> CreateList { get; set; }
        public List<GetProjectProcessResultDto.ResponseFailDto> FailedList { get; set; }
    }
}