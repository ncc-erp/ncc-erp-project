using NccCore.Paging;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class InputGetResourceDto : GridParam
    {
        public List<UserType> UserTypes { get; set; }
        public List<long> BranchIds { get; set; }
        public List<long> PositionIds { get; set; }
        public List<long> SkillIds { get; set; }
        public PlanStatus PlanStatus { get; set; }
        public bool IsAndCondition { get; set; }
    }
}
