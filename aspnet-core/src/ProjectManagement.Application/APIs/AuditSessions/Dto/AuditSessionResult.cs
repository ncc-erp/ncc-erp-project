using Abp.Domain.Entities;
using NccCore.Anotations;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.AuditSessions.Dto
{
    public class AuditSessionResultDto : Entity<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public long CountProjectCreate { get; set; }
        public long CountProjectCheck { get; set; }
        public long CountFail { get; set; }
    }
}
