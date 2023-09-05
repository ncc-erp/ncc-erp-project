using Abp.Authorization;
using ProjectManagement.Authorization.Roles;
using ProjectManagement.Authorization.Users;

namespace ProjectManagement.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
