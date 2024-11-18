using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class BillUserSkillDto
    {
        public long SkillId { get; set; }
        public string SkillName { get; set; }
        public float SkillRank { get; set; }
        public string SkillNote { get; set; }
    }
}