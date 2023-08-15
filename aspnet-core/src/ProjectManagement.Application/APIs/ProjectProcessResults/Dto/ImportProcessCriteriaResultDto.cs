using System.Collections.Generic;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class ImportProcessCriteriaResultDto
    {
        public GetProjectProcessResultDto.AuditInfo AuditInfo { get; set; }
        public List<GetProjectProcessCriteriaResultDto> CriteriaResult { get; set; }
        public List<GetProjectProcessResultDto.ResponseFailDto> FailedList { get; set; }
    }
}