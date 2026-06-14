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

        [ForeignKey(nameof(AccountTypeId))]
        public AccountType AccountType { get; set; }
        public long AccountTypeId { get; set; }

        [ForeignKey(nameof(AccountAssetCreatorId))]
        public AccountAssetCreator AccountAssetCreator { get; set; }
        public long AccountAssetCreatorId { get; set; }
        public string AssetName { get; set; }
        public string TypeLogin { get; set; }
    }
}
