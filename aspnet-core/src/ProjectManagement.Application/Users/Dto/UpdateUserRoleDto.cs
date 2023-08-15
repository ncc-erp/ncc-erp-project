using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Users.Dto
{
    public class UpdateUserRoleDto
    {
        public long UserId { get; set; }
        public string[] RoleNames { get; set; }

    }
}
