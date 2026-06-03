using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class OffboardUser : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public long UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public long ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public CheckOffboardStatus CheckOffboardStatus { get; set; }
        [MaxLength(4000)]
        public string OffboardChecklistJson { get; set; }
        [MaxLength(4000)]
        public string HistoryAsset { get; set; }
        public OffboardStatus OffboardStatus { get; set; }
        public DateTime OffboardDate { get; set; }

    }
}
