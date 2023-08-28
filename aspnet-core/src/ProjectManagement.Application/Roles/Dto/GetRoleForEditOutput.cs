using System.Collections.Generic;
using static ProjectManagement.Authorization.GrantPermissionRoles;

namespace ProjectManagement.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<SystemPermission> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}