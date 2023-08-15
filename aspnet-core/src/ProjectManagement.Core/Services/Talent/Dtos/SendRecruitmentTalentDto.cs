using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.Talent.Dtos
{
    public class SendRecruitmentTalentDto
    {
        public long ResourceRequestId { get; set; }
        public long SubPositionId { get; set; }
        public long BranchId { get; set; }
        public int Quantity { get; set; }
        public List<string> SkillNames { get; set; }
        public UserLevel Level { get; set; }
        public DateTime TimeNeed { get; set; }
        public Priority Priority { get; set; }
        public string Note { get; set; }
    }
}
