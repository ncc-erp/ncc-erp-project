using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ProjectManagement.Configuration;
using Abp.Threading.BackgroundWorkers;
using ProjectManagement.BackgroundWorkers;

namespace ProjectManagement.Web.Host.Startup
{
    [DependsOn(
       typeof(ProjectManagementWebCoreModule))]
    public class ProjectManagementWebHostModule : AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public ProjectManagementWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {

            IocManager.RegisterAssemblyByConvention(typeof(ProjectManagementWebHostModule).GetAssembly());
            //register background worker
            var wokerManager = IocManager.Resolve<IBackgroundWorkerManager>();
            // inform pm to send weekly report
            wokerManager.Add(IocManager.Resolve<InformPmWorker>());
        }
    }
}
