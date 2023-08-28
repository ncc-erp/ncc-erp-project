using Abp.Domain.Entities;
using NccCore.Anotations;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class GetTrainingProjectDto : Entity<long>
    {
        [ApplySearchAttribute]
        public string Name { get; set; }
        [ApplySearchAttribute]
        public string Code { get; set; }
        public long? ClientId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ProjectStatus Status { get; set; }
        //public string Status { get; set; }
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
        public UserLevel PmUserLevel { get; set; }
        public Branch PmBranch { get; set; }
        public PMReportProjectStatus IsSent { get; set; }
        public DateTime? TimeSendReport { get; set; }
        public DateTime? DateSendReport { get; set; }
        public string Evaluation { get; set; }
        public string PmBranchColor { get; set; }
        public string PmBranchDisplayName { get; set; }
        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }
        public bool? IsRequiredWeeklyReport { get; set; }
    }
}
