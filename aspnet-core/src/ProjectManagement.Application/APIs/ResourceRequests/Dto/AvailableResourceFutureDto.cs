using Abp.Application.Services.Dto;
using NccCore.Anotations;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class AvailableResourceFutureDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long Projectid { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
        public byte Use { get; set; }
        public string EmailAddress { get; set; }
        [ApplySearchAttribute]
        public string FullName { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
    }
}
