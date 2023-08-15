using Abp.Application.Services.Dto;
using NccCore.Anotations;
using ProjectManagement.Utils;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class GetProjectUserBillDto : EntityDto<long>
    {
        public long UserId { get; set; }
        [ApplySearchAttribute]
        public string UserName { get; set; }
        public long ProjectId { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        public string AccountName { get; set; }
        public string BillRole { get; set; }
        public float BillRate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string CurrencyName { get; set; }
        public string Note { get; set; }
        public string shadowNote { get; set; }
        public bool isActive { get; set; }
        public string EmailAddress { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public UserLevel UserLevel { get; set; }
        public byte? LastInvoiceNumber { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public ChargeType? ChargeType { get; set; }
        public string BillAccountName => string.IsNullOrEmpty(AccountName) ? FullName : AccountName;
        public string ChargeTypeName => CommonUtil.ChargeTypeName(ChargeType);
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }

    }
}
