using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System.ComponentModel.DataAnnotations;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.AuditResults.Dto
{
    [AutoMapTo(typeof(AuditResult))]
    public class AuditResultDto :Entity<long>
    {
        public long AuditSessionId { get; set; }

        public long ProjectId { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }

        public long PMId { get; set; }
        public AuditResultStatus status { get; set; }
    }
}
