using NccCore.Anotations;
using NccCore.Extension;
using NccCore.Paging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Services.HRM.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class BillInfoDto
    {
        public GetUserBillDto UserInfor { get; set; }
        public List<GetProjectBillDto> Projects { get; set; }
    }

    public class GetUserBillDto : IComparable<GetUserBillDto>
    {
        public long UserId { get; set; }
        [ApplySearchAttribute]
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        public Branch Branch { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }
        [ApplySearchAttribute]
        public string EmailAddress { get; set; }
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }

        int IComparable<GetUserBillDto>.CompareTo(GetUserBillDto other)
        {
            return this.FullName.CompareTo(other.FullName);
        }
    }
    public class GetProjectBillDto : IComparable<GetProjectBillDto>
    {
        public long ProjectId { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        [ApplySearchAttribute]
        public string AccountName { get; set; }
        public float BillRate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Note { get; set; }
        public string shadowNote { get; set; }
        public bool isActive { get; set; }
        public ChargeType? ChargeType { get; set; }
        public string CurrencyCode { get; set; }
        public string FullName { get; set; }
        public string BillAccountName => string.IsNullOrEmpty(AccountName) ? FullName : AccountName;
        public string ProjectCode { get; set; }

        public string BillRatePerChargeType => $"{this.BillRate} {this.CurrencyCode} / {ChargeTypeString()}";
        public string ChargeTypeString()
        {
            switch (this.ChargeType)
            {
                case ProjectEnum.ChargeType.Daily:
                    return "Daily";
                case ProjectEnum.ChargeType.Hourly:
                    return "Hourly";
                case ProjectEnum.ChargeType.Monthly:
                    return "Monthly";
                default:
                    return "";
            }
        }

        int IComparable<GetProjectBillDto>.CompareTo(GetProjectBillDto other)
        {
            return this.ProjectName.CompareTo(other.ProjectName);
        }
    }

    public class InputGetBillInfoDto
    {
        public string SearchText { get; set; }
        public long? ProjectId { get; set; }
        public JoinOutStatus? JoinOutStatus { get; set; }
        public ChargeStatus? ChargeStatus { get; set; }
        public GridParam GirdParam { get; set; }
        public IDictionary<string, SortDirection> SortParams { get; set; }
    }

    public class ProjectUserAccountPlanningDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

}
