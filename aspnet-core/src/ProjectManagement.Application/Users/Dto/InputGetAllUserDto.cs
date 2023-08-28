using NccCore.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Users.Dto
{
    public class InputGetAllUserDto: GridParam
    {
        public List<long> SkillIds { get; set; }
    }
}
