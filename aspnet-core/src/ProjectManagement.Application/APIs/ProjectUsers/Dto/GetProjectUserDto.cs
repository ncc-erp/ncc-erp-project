using Abp.Application.Services.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUsers.Dto
{
    public class GetProjectUserDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public string FullName { get; set; }
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectRole { get; set; }
        public byte AllocatePercentage { get; set; }
        public DateTime StartTime { get; set; }
        public string Status { get; set; }
        public bool IsExpense { get; set; }
        public long? ResourceRequestId { get; set; }
        public string ResourceRequestName { get; set; }
        public long PMReportId { get; set; }
        public string PMReportName { get; set; }
        public bool IsFutureActive { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public string Note { get; set; }
        public string PMName { get; set; }
    }
}
