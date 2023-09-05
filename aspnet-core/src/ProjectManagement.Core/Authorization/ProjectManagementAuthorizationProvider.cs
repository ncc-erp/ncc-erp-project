using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using static ProjectManagement.Authorization.GrantPermissionRoles;

namespace ProjectManagement.Authorization
{
    public class ProjectManagementAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            foreach (var permission in SystemPermission.ListPermissions)
            {
                context.CreatePermission(permission.Name, L(permission.DisplayName), multiTenancySides: permission.MultiTenancySides);
            }
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ProjectManagementConsts.LocalizationSourceName);
        }
    }
}
