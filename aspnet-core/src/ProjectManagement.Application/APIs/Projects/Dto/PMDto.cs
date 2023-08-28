using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Utils;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class PMDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string AvatarPath { get; set; }
        public string AvatarFullPath => FileUtils.FullFilePath(AvatarPath);
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public Branch Branch { get; set; }
    }
}