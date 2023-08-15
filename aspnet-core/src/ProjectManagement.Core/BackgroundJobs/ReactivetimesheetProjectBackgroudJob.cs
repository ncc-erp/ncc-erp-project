using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using ProjectManagement.Manager.TimesheetProjectManager;
using ProjectManagement.Manager.TimesheetProjectManager.Dto;

namespace ProjectManagement.BackgroundJobs
{
    public class ReactivetimesheetProjectBackgroudJob : BackgroundJob<ReactiveTimesheetBGJDto>, ITransientDependency
    {
        private readonly ReactiveTimesheetProject _reactiveTimesheet;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWork;

        public ReactivetimesheetProjectBackgroudJob(ReactiveTimesheetProject reactiveTimesheet, IAbpSession abpSession, IUnitOfWorkManager unitOfWork)
        {
            _reactiveTimesheet = reactiveTimesheet;
            _abpSession = abpSession;
            _unitOfWork = unitOfWork;
        }

        [UnitOfWork]
        public override void Execute(ReactiveTimesheetBGJDto args)

        {
            _abpSession.Use(args.TenantId, args.CurrentUserLoginId);
            var uow = _unitOfWork.Current;

            using (uow.SetTenantId(args.TenantId))
            {
                _reactiveTimesheet.DeactiveTimesheetProject(args.TimesheetProjectId);
                uow.SaveChanges();
            }
        }
    }
}