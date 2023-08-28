using NccCore.Anotations;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.Services.HRM.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class BillInfoDto
    {
        public GetUserBillDto UserInfor { get; set; }
        public List<GetProjectBillDto> Projects { get; set; }
    }

    public class GetUserBillDto
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
    }

    public class GetProjectBillDto
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
    }

    public class InputGetBillInfoDto
    {
        public string SearchText { get; set; }
        public long? ProjectId { get; set; }
        public JoinOutStatus? JoinOutStatus { get; set; }
        public ChargeStatus? ChargeStatus { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public GridParam GirdParam { get; set; }
    }

    public class ProjectUserAccountPlanningDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

}
