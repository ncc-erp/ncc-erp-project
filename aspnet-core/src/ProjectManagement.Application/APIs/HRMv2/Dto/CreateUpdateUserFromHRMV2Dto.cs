using ProjectManagement.APIs.HRM.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.HRMv2.Dto
{
    public class CreateUpdateUserFromHRMV2Dto
    {
        public UserType Type { get; set; }
        public string EmailAddress { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }
        public string BranchCode { get; set; }
        public string LevelCode { get; set; }
        public string PositionCode { get; set; }
        public DateTime WorkingStartDate { get; set; }
        public List<string> SkillNames { get; set; }
        public UserLevel Level
        {
            get
            {
                var userLevel = CommonUtil.GetUserLevelByLevelCode(LevelCode);
                if (userLevel > UserLevel.Principal || userLevel < UserLevel.Intern_0)
                {
                    userLevel = Type == UserType.Internship ? UserLevel.Intern_0 : UserLevel.FresherMinus;
                }
                return userLevel;
            }
        }

    }
}
