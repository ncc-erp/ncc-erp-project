using NccCore.Anotations;
using NccCore.Extension;
using NccCore.Paging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ProjectManagement.APIs.ProjectProcessResults.Dto;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Services.HRM.Dto;
using ProjectManagement.Services.ProjectUserBill.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Text;
using static ProjectManagement.Constants.Enum.ClientEnum;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class BillInfoDto
    {
        public GetUserBillDto UserInfor { get; set; }
        public List<GetProjectBillDto> Projects { get; set; }
        public float TotalHeadCount { get; set; }
    }

    public class GetUserBillDto : IEquatable<GetUserBillDto>
    {
        public long UserId { get; set; }
        public string AvatarPath { get; set; }
        public string FullName { get; set; }
        public Branch Branch { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }
        public string EmailAddress { get; set; }
        public string SimplizeEmailAddress => this.EmailAddress.Split('@')[0];
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }

        public bool Equals(GetUserBillDto other)
        {
            if (other == null) return false;
            return this.UserId == other.UserId; // Compare based on Id
        }

        public override bool Equals(object obj)
        {
            if (obj is GetUserBillDto other)
            {
                return this.Equals(other);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.UserId.GetHashCode();
        }

        //public bool Equals(GetUserBillDto x, GetUserBillDto y)
        //{
        //    if (x == null && y == null)
        //        return true;
        //    if (x == null || y == null)
        //        return false;
        //    return x.UserId == y.UserId;
        //}

        //public int GetHashCode(GetUserBillDto obj)
        //{
        //    return obj.UserId.GetHashCode();
        //}
    }

    public class GetProjectBillDto
    {
        public long BillId { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string AccountName { get; set; }
        public float BillRate { get; set; }
        public float HeadCount {  get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Note { get; set; }
        public bool isActive { get; set; }
        public bool isExpose { get; set; }
        public ChargeType? ChargeType { get; set; }
        public string CurrencyCode { get; set; }
        public string ProjectCode { get; set; }
        public long? ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string RateDisplay => $"{CommonUtil.FormatMoney(this.BillRate)} {this.CurrencyCode}/{CommonUtil.ChargeTypeShortName(this.ChargeType)}";
        public IEnumerable<GetUserInfo> LinkedResources { get; set; }

    }

    public class InputGetBillInfoDto : GridParam
    {
        public long? ProjectId { get; set; }
        public long? ClientId { get; set; }
        public bool? IsCharge { get; set; }
        public ProjectStatus? ProjectStatus { get; set; }
    }

    public class ProjectUserAccountPlanningDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ProjectClientPlanningDto
    {
        public long? Id { get; set; }
        public string Name { get; set; }
    }
}

