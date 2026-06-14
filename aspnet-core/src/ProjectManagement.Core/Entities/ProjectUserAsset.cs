using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProjectUserAsset : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(UserId))]
        public long UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(ProjectAssetId))]
        public long ProjectAssetId { get; set; }
        public ProjectAsset ProjectAsset { get; set; }
    }
}
