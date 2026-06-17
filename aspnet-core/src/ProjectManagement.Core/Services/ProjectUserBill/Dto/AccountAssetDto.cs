using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    [AutoMapTo(typeof(AccountAsset))]
    public class AccountAssetDto
    {
        public long Id { get; set; }
        public long ProjectUserBillId { get; set; }
        public long ProjectAssetId { get; set; }
        public long ProjectAssetTypeId { get; set; }
        public string ProjectAssetTypeName { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public long AccountAssetCreatorId { get; set; }
        public string AccountAssetCreatorName { get; set; }
        public long TypeLoginId { get; set; }
        public string TypeLoginName { get; set; }
        public string AssetName { get; set; }
    }
}
