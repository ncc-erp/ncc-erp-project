using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Users.Dto
{
    public class UpdateUserSkillDto
    {
        public long UserId { get; set; }
        public List<SkillPoint> UserSkills { get; set; }

    }

    public struct SkillPoint
    {
        public long SkillId { get; set; }
        public SkillRank SkillRank { get; set; }
    }
}
