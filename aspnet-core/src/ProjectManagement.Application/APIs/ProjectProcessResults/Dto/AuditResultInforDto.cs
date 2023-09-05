using ProjectManagement.Constants.Enum;
using System;

namespace ProjectManagement.APIs.ProjectProcessResults.Dto
{
    public class AuditResultInforDto
    {
        public string AuditDate { get; set; }
        public ProjectEnum.ProjectScoreKPIStatus Status { get; set; }
        public int Score { get; set; }
        public string Note { get; set; }
        public string LastModifyTime { get; set; }
        public long? LastModifyUser { get; set; }
        public string LastModifyUserName { get; set; }
        public long PmId { get; set; }
        public string PMName { get; set; }
        public long Id { get; set; }
    }
}