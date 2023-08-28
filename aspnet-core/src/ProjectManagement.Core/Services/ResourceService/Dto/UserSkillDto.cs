using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class UserSkillDto
    {
        public long UserId { get; set; }
        public long SkillId { get; set; }
        public string SkillName { get; set; }
        public SkillRank SkillRank { get; set; }
    }
}
