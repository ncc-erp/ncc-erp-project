using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using ProjectManagement.Manager.TimesheetManagers;

namespace ProjectManagement.BackgroundJobs
{
    public class CloseTimeSheetBackgroundJob : BackgroundJob<TimesheetBGJDto>, ITransientDependency
    {
        private readonly CloseTimesheet _closeTimesheet;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWork;

        public CloseTimeSheetBackgroundJob(CloseTimesheet closeTimesheet, IAbpSession abpSession, IUnitOfWorkManager unitOfWork)
        {
            _closeTimesheet = closeTimesheet;
            _abpSession = abpSession;
            _unitOfWork = unitOfWork;
        }

        [UnitOfWork]
        public override void Execute(TimesheetBGJDto args)
        {
            _abpSession.Use(args.TenantId, args.CurrentUserLoginId);
            var uow = _unitOfWork.Current;

            using (uow.SetTenantId(args.TenantId))
            {
                _closeTimesheet.DeactiveTimesheet(args.TimesheetId);
                uow.SaveChanges();
            }
        }
    }
}