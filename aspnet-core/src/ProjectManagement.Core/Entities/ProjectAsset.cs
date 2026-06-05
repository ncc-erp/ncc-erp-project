using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class ProjectAsset : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public long ProjectId { get; set; }
        public Project Project { get; set; }
        public string AssetName { get; set; }
        public ICollection<ProjectUserAsset> ProjectUserAssets { get; set; }
    }
}
