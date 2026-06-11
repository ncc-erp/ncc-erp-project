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
        public long ProjectId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }

        [ForeignKey(nameof(ProjectResourceId))]
        public ProjectResource ProjectResource { get; set; }
        public long ProjectResourceId { get; set; }
        public string AssetName { get; set; }
        public ICollection<ProjectUserAsset> ProjectUserAssets { get; set; }
    }
}
