using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    [AutoMapTo(typeof(AccountResource))]
    public class AccountResourceDto
    {
        public long Id { get; set; }
        public long ProjectUserBillId { get; set; }
        public long AccountTypeId { get; set; }
        public string AccountTypeName { get; set; }
        public long CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string TypeLogin { get; set; }
        public string AssetName { get; set; }
    }
}
