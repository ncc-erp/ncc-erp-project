using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ProjectManagement.Entities
{
    public class AccountAsset : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }

        [ForeignKey(nameof(ProjectUserBillId))]
        public ProjectUserBill ProjectUserBill { get; set; }
        public long ProjectUserBillId { get; set; }

        [ForeignKey(nameof(ProjectAssetId))]
        public ProjectAsset ProjectAsset { get; set; }
        public long ProjectAssetId { get; set; }
    }
}
