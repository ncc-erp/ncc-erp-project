using Abp.AutoMapper;
using Abp.Domain.Entities;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;

namespace ProjectManagement.APIs.AuditSessions.Dto
{
    [AutoMapTo(typeof(AuditSession))]
    public class AuditSessionDto: Entity<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
