using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.HRM.Dto
{
    public class UpdateUserFromHRMDto
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string AvatarPath { get; set; }
        public string EmailAddress { get; set; }
        public string UserCode { get; set; }
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public Branch Branch { get; set; }
        public string BranchCode { get; set; }
    }
}
