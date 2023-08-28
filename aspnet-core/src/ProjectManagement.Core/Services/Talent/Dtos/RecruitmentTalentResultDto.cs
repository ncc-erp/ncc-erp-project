using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Talent.Dtos
{
    public class RecruitmentTalentResultDto
    {
        public bool IsRecruitmentSend { get; set; }
        public string RecruitmentUrl { get => CommonUtil.GetPathSendRecuitment(PathRecruitment); }
        public string PathRecruitment { get; set; }
    }
}
