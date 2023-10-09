using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using NccCore.IoC;
using Newtonsoft.Json;
using ProjectManagement.BackgroundJobs;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
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

        public void CreateReqCloseTimesheetBGJ(Timesheet timesheet, DateTime? closeTime)
        {
            if (closeTime <= timesheet.CreationTime)
                throw new UserFriendlyException("Close time must be greater than creation time!");
            DeleteOldRequestInBackgroundJob(timesheet.Id);
            if (closeTime == null) return;
            var timesheetBGJ = new TimesheetBGJDto
            {
                TimesheetId = timesheet.Id,
                CurrentUserLoginId = AbpSession.UserId,
                TenantId = AbpSession.TenantId
            };
            var delays = (closeTime.Value - DateTime.Now).TotalMilliseconds;
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

        public Dictionary<long, string> GetCloseTimeInBackgroundJobs()
        {
            var closeTimesheetBGJob = typeof(CloseTimeSheetBackgroundJob).FullName;
            var filterEmployee = "TimesheetId";
            var nextTryTimes = _storeJob.GetAll()
                 .Where(s => s.JobType.Contains(closeTimesheetBGJob) && s.JobArgs.Contains(filterEmployee))
                 .Select(s => new
                 {
                     ID = JsonConvert.DeserializeObject<TimesheetBGJDto>(s.JobArgs).TimesheetId,
                     Time = s.NextTryTime.ToString("dd-MM-yyyy HH:mm")
                 })
                 .ToDictionary(s => s.ID, s => s.Time);
            return nextTryTimes;
        }
    }
}