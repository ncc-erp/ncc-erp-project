using Abp.Application.Services.Dto;
using NccCore.Anotations;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class GetProjectDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        [ApplySearchAttribute]
        public string Code { get; set; }
        public ProjectType ProjectType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectStatus Status { get; set; }
        //public string Status { get; set; }
        public long? ClientId { get; set; }
        [ApplySearchAttribute]
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public long? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public bool? IsCharge { get; set; }
        public ChargeType? ChargeType { get; set; }
        public long PmId { get; set; }
        [ApplySearch]
        public string PmName { get; set; }
        public string PmFullName { get; set; }
        [ApplySearch]
        public string PmEmailAddress { get; set; }
        public string PmUserName { get; set; }
        public string PmAvatarPath { get; set; }
        public string PmAvatarFullPath => FileUtils.FullFilePath(PmAvatarPath);
        public UserType PmUserType { get; set; }
        public Branch PmBranch { get; set; }
        public UserLevel PmUserLevel { get; set; }
        public PMReportProjectStatus IsSent { get; set; }
        public DateTime? TimeSendReport { get; set; }
        public DateTime? DateSendReport { get; set; }
        public bool RequireTimesheetFile { get; set; }
        public List<GetBillInfoDto> BillInfo { get; set; }
        public string PmBranchColor { get; set; }
        public string PmBranchDisplayName { get; set; }
        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }
        public bool IsRequiredWeeklyReport { get; set; }
        public long? ParentInvoiceId { get; set; } 
        public bool IsParentInvoiceIdExist { get; set; } 
        public IEnumerable<SubProjectInvoiceDto> ParentInvoices { get; set; }
        public List<UserJoinProjectDto> CurrentResources { get; set; }

    }

    public class UserJoinProjectDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string AvatarPath { get; set; }

        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public Branch Branch { get; set; }
        public bool IsAvtive { get; set; }
        public byte AllocatePercentage { get; set; }
        public DateTime StartTime { get; set; }
        public int? StarRate { get; set; }

        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }

        public ProjectUserStatus PUStatus { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public List<UserSkillDto> UserSkills { get; set; }

        public long? PMReportId { get; set; }
        public bool IsPool { get; set; }
        public string Note { get; set; }

        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
    }
}