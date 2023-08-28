using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ProjectCriteriaResult : FullAuditedEntity<long>, IMayHaveTenant
    {
        public string Note { get; set; }

        [ForeignKey(nameof(PMReportId))]
        public PMReport PMReport { get; set; }

        public long PMReportId { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        [ForeignKey(nameof(ProjectCriteriaId))]
        public ProjectCriteria ProjectCriteria { get; set; }

        public long ProjectCriteriaId { get; set; }

        public long ProjectId { get; set; }

        public ProjectCriteriaResultStatus Status { get; set; }

        public int? TenantId { get; set; }
    }
}