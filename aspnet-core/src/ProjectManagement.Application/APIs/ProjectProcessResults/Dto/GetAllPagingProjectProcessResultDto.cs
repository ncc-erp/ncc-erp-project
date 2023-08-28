using NccCore.Anotations;
using ProjectManagement.Constants.Enum;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class GetAllPagingProjectProcessResultDto
    {
        public long ProjectId { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        [ApplySearchAttribute]
        public string ProjectCode { get; set; }
        [ApplySearchAttribute]
        public string ClientName { get; set; }
        [ApplySearchAttribute]
        public string ClientCode { get; set; }
        [ApplySearchAttribute]
        public string PMName { get; set; }
        public List<AuditResultInforDto> AuditResultInfor { get; set; }
        public ProjectType ProjectType { get; set; }
        public ProjectEnum.ProjectStatus ProjectStatus { get; set; }
        public long PmId { get; set; }
        public long ClientId { get; set; }
    }

    public class GetPMDto
    {
        public long Id { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
    }

    public class GetClientDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}