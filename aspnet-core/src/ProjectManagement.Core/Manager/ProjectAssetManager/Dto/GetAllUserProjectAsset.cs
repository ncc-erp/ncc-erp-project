using Abp.Application.Services.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Manager.ProjectAssetManager.Dto
{
    public class GetAllUserProjectAsset : EntityDto<long>
    {
        public long UserId { get; set; }
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public Branch Branch { get; set; }
        public string FullName { get; set; }
        public string BranchColor { get; set; }
        public string BranchDisplayName { get; set; }
        public long? PositionId { get; set; }
        public string PositionColor { get; set; }
        public string PositionName { get; set; }
        public UserLevel UserLevel { get; set; }
        public string UserTypeName => CommonUtil.UserTypeName(UserType);
        public List<string> ProjectAssets { get; set; }
    }
}
