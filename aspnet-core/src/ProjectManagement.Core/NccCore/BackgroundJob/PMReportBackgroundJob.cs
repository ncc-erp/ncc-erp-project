using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using ProjectManagement.Entities;
using System;
using System.Threading.Tasks;

namespace ProjectManagement.NccCore.BackgroundJob
{
   public  class PMReportBackgroundJob : AsyncBackgroundJob<PMReportBackgroundJobArgs>, ITransientDependency
    {
        readonly IRepository<PMReport, long> _pmreport;
        public PMReportBackgroundJob(IRepository<PMReport, long> pmreport)
        {
            _pmreport = pmreport;
        }
        [UnitOfWork]
        protected override async Task ExecuteAsync(PMReportBackgroundJobArgs args)
        {
            Logger.Info("PMReport background trigger!");
            try
            {
                using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MustHaveTenant, AbpDataFilters.MayHaveTenant))
                {
                    var pmReport = await _pmreport.GetAsync(args.PMReportId);
                    pmReport.PMReportStatus = args.PMReportStatus;
                    await _pmreport.UpdateAsync(pmReport);
                }               
                Logger.Info("PMReport background success!.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}
