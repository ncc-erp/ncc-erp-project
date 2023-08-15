using Abp.Application.Services.Dto;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class GetTimeSheetProjectBillDto : EntityDto<long>
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
        //public CurrencyCode Currency { get; set; }
        public string Note { get; set; }
        public string ShadowNote { get; set; }
        public bool IsActive { get; set; }
        public string EmailAddress { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public float WorkingTime { get; set; }
        public string ProjectBillInfomation { get; set; }
        public UserLevel? UserLevel { get; set; }
        public string Currency { get; set; }
        public ChargeType? ChargeType { get; set; }
        public string ChargeTypeName => CommonUtil.ChargeTypeName(ChargeType);
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public string BillAccountName => string.IsNullOrEmpty(AccountName) ? FullName : AccountName;
        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }
    }
}
