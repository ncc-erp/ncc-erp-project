using Abp.Application.Services.Dto;
using NccCore.Uitls;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceRequestService.Dto
{ 
    public class UserBaseDto: EntityDto<long>
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public UserLevel UserLevel { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public long? PositionId { get; set; }
        public string PositionName { get; set; }
        public string PositionColor { get; set; }

        public string LevelName
        {
            get
            {
                return CommonUtil.UserLevelName(UserLevel);
            }
        }

        public string KomuInfo()
        {
            return $"{FullName} [{BranchName} - {UserTypeName} - {LevelName}]";
        }

        public string BranchName
        {
            get
            {
                return CommonUtil.BranchName(Branch);
            }
        }

        public string UserTypeName
        {
            get
            {
                return CommonUtil.UserTypeName(UserType);
            }
        }
        
    }

}
