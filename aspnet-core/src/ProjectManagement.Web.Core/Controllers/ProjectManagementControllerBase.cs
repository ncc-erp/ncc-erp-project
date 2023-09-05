using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ProjectManagement.Controllers
{
    public abstract class ProjectManagementControllerBase: AbpController
    {
        protected ProjectManagementControllerBase()
        {
            LocalizationSourceName = ProjectManagementConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
