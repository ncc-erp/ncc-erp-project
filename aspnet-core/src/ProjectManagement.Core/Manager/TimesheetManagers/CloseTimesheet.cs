using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Timing;
using NccCore.IoC;
using ProjectManagement.BackgroundJobs;
using ProjectManagement.Entities;
using System;
using System.Linq;

namespace ProjectManagement.Manager.TimesheetManagers
{
    public class CloseTimesheet : BaseManager
    {
        private readonly IRepository<BackgroundJobInfo, long> _storeJob;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public CloseTimesheet(IWorkScope workScope, IRepository<BackgroundJobInfo, long> storeJob, IBackgroundJobManager backgroundJobManager) : base(workScope)
        {
            _storeJob = storeJob;
            _backgroundJobManager = backgroundJobManager;
        }

        public void CreateReqCloseTimesheetBGJ(Timesheet timesheet)
        {
            DeleteOldRequestInBackgroundJob(timesheet.Id);
            var timesheetBGJ = new TimesheetBGJDto
            {
                TimesheetId = timesheet.Id,
                CurrentUserLoginId = AbpSession.UserId,
                TenantId = AbpSession.TenantId
            };
            var nextMonth = new DateTime(timesheet.CreationTime.AddMonths(1).Year, timesheet.CreationTime.AddMonths(1).Month, 1);
            var delays = (nextMonth - timesheet.CreationTime).TotalMilliseconds;
            _backgroundJobManager.Enqueue<CloseTimeSheetBackgroundJob, TimesheetBGJDto>(
                    timesheetBGJ, BackgroundJobPriority.High, TimeSpan.FromMilliseconds(delays));
            //var delays = 1;
            //_backgroundJobManager.Enqueue<CloseTimeSheetBackgroundJob, TimesheetBGJDto>(
            //        timesheetBGJ, BackgroundJobPriority.High, TimeSpan.FromMinutes(delays));
        }

        public void ReOpenTimesheet(Timesheet timesheet, DateTime closeTime)
        {
            DeleteOldRequestInBackgroundJob(timesheet.Id);
            var timesheetBGJ = new TimesheetBGJDto
            {
                TimesheetId = timesheet.Id,
                CurrentUserLoginId = AbpSession.UserId,
                TenantId = AbpSession.TenantId
            };
            var delays = (closeTime - Clock.Provider.Now).TotalMilliseconds;
            _backgroundJobManager.Enqueue<CloseTimeSheetBackgroundJob, TimesheetBGJDto>(
                    timesheetBGJ, BackgroundJobPriority.High, TimeSpan.FromMilliseconds(delays));
            //var delays = 1;
            //_backgroundJobManager.Enqueue<CloseTimeSheetBackgroundJob, TimesheetBGJDto>(
            //        timesheetBGJ, BackgroundJobPriority.High, TimeSpan.FromMinutes(delays));
        }

        public void DeleteOldRequestInBackgroundJob(long timesheetId)
        {
            var jobTypeNameOfRequestToQuit = typeof(CloseTimeSheetBackgroundJob).FullName;

            var filterEmployee = $"\"TimesheetId\":{timesheetId},";
            _storeJob.GetAll()
                 .Where(s => s.JobType.Contains(jobTypeNameOfRequestToQuit))
                 .Where(s => s.JobArgs.Contains(filterEmployee))
                 .Select(s => s.Id)
                 .ToList().ForEach(id => _backgroundJobManager.Delete(id.ToString()));
        }

        public void DeactiveTimesheet(long timesheetId)
        {
            var item = WorkScope.Get<Timesheet>(timesheetId);
            item.IsActive = false;
        }
    }
}