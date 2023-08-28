using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectManagement.EntityFrameworkCore;
using ProjectManagement.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ProjectManagement.Web.Tests
{
    [DependsOn(
        typeof(ProjectManagementWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class ProjectManagementWebTestModule : AbpModule
    {
        public ProjectManagementWebTestModule(ProjectManagementEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ProjectManagementWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(ProjectManagementWebMvcModule).Assembly);
        }
    }
}