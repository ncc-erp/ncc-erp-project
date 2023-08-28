using Abp.Domain.Entities;
using NccCore.Anotations;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.AuditSessions.Dto
{
    public class AuditSessionDetailDto : Entity<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long ProjectId { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        [ApplySearchAttribute]
        public string PmName { get; set; }
        [ApplySearchAttribute]
        public string PmNameNormal { get; set; }
        public string AuditResultStatus { get; set; }
        public long CountProjectCreate { get; set; }
        public long CountProjectCheck { get; set; }
        public long CountFail { get; set; }
        public string Status { get; set; }
    }
}
