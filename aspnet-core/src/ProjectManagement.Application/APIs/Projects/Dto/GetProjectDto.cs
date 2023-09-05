using Abp.Application.Services.Dto;
using NccCore.Anotations;
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
        public bool? IsRequiredWeeklyReport { get; set; }
    }
}