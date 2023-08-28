using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class BaseUserInfo
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string BranchName { get; set; }
        public string AvatarPath { get; set; }

        public UserType UserType { get; set; }

        public string FullAvatarPath => FileUtils.FullFilePath(AvatarPath);
        public string UserTypeName => CommonUtil.UserTypeName(UserType);
    }
}
