using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using ProjectManagement.Authorization.Users;
using ProjectManagement.MultiTenancy;
using Abp.Dependency;
using Abp.ObjectMapping;
using NccCore.IoC;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using ProjectManagement.Configuration;
using Abp.UI;
using Microsoft.AspNetCore.Http;

namespace ProjectManagement
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class ProjectManagementAppServiceBase : ApplicationService
    {
        public TenantManager TenantManager { get; set; }

        public UserManager UserManager { get; set; }
        protected IWorkScope WorkScope { get; set; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        protected ProjectManagementAppServiceBase()
        {
            LocalizationSourceName = ProjectManagementConsts.LocalizationSourceName;
            WorkScope = IocManager.Instance.Resolve<IWorkScope>();
            ObjectMapper = IocManager.Instance.Resolve<IObjectMapper>();
            UserManager = IocManager.Instance.Resolve<UserManager>();
            TenantManager = IocManager.Instance.Resolve<TenantManager>();
            HttpContextAccessor = IocManager.Instance.Resolve<IHttpContextAccessor>();

        }

        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        protected virtual async Task<User> GetUserByEmailAsync(string emailAddress)
        {
           return await WorkScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == emailAddress.ToLower().Trim())
                .FirstOrDefaultAsync();
        }
        protected virtual long GetUserIdByEmail(string emailAddress)
        {
            return WorkScope.GetAll<User>()
                 .Where(x => x.EmailAddress.ToLower().Trim() == emailAddress.ToLower().Trim())
                 .Select(x => x.Id)
                 .FirstOrDefault();
        }

        protected  List<User> GetUsers(List<string> emails)
        {
            return WorkScope.GetAll<User>()
                 .Where(x => emails.Contains(x.EmailAddress))
                 .ToList();
        }
        protected void CheckSecurityCode()
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = HttpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode == securityCodeHeader)
                return;

            throw new UserFriendlyException($"SecretCode does not match! {secretCode.Substring(secretCode.Length - 3)} != {securityCodeHeader}");
        }
    }
}
