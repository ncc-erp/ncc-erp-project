using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using NccCore.IoC;
using ProjectManagement.BackgroundJobs;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Manager.TimesheetProjectManager.Dto;
using System;
using System.Linq;

namespace ProjectManagement.Manager.TimesheetProjectManager
{
    public class ReactiveTimesheetProject : BaseManager
    {
        private readonly IRepository<BackgroundJobInfo, long> _storeJob;
        private readonly IBackgroundJobManager _backgroundJobManager;

        public ReactiveTimesheetProject(IWorkScope workScope, IRepository<BackgroundJobInfo, long> storeJob, IBackgroundJobManager backgroundJobManager) : base(workScope)
        {
            _storeJob = storeJob;
            _backgroundJobManager = backgroundJobManager;
        }

        public void CreateReqDeActiveTimesheetProjectBGJ(long timesheetProjectId, DateTime? closeDate)
        {
            DeleteOldRequestInBackgroundJob(timesheetProjectId);
            var reactiveTimesheetBGJDto = new ReactiveTimesheetBGJDto
            {
                TimesheetProjectId = timesheetProjectId,
                CurrentUserLoginId = AbpSession.UserId,
                TenantId = AbpSession.TenantId
            };
            var delays = double.Parse(SettingManager.GetSettingValue(AppSettingNames.ActiveTimesheetProjectPeriod));
            if (closeDate.HasValue)
            {
                if ((closeDate.Value - Clock.Provider.Now).TotalMilliseconds < 0)
                {
                    throw new UserFriendlyException("Thời gian đóng phải lớn hơn thời gian hiện tại");
                }
                delays = (closeDate.Value - Clock.Provider.Now).TotalMilliseconds;
            }

            _backgroundJobManager.Enqueue<ReactivetimesheetProjectBackgroudJob, ReactiveTimesheetBGJDto>(
                    reactiveTimesheetBGJDto, BackgroundJobPriority.High, TimeSpan.FromMilliseconds(delays));

        }

        public void DeleteOldRequestInBackgroundJob(long timesheetProjectId)
        {
            var jobTypeNameOfRequestToQuit = typeof(ReactivetimesheetProjectBackgroudJob).FullName;

            var filterTimesheetProject = $"\"TimesheetProjectId\":{timesheetProjectId},";
            _storeJob.GetAll()
                 .Where(s => s.JobType.Contains(jobTypeNameOfRequestToQuit))
                 .Where(s => s.JobArgs.Contains(filterTimesheetProject))
                 .Select(s => s.Id)
                 .ToList().ForEach(id => _backgroundJobManager.Delete(id.ToString()));
        }

        public string GetCloseTimeBackgroundJob(long timesheetProjectId)
        {
            var jobTypeNameOfRequestToQuit = typeof(ReactivetimesheetProjectBackgroudJob).FullName;

            var filterTimesheetProject = $"\"TimesheetProjectId\":{timesheetProjectId},";
            return _storeJob.GetAll()
                 .Where(s => s.JobType.Contains(jobTypeNameOfRequestToQuit))
                 .Where(s => s.JobArgs.Contains(filterTimesheetProject))
                 .Select(s => s.NextTryTime).FirstOrDefault().ToString("dd-MM-yyyy HH:mm");
        }

        public void DeactiveTimesheetProject(long timesheetProjectId)
        {
            var item = WorkScope.Get<TimesheetProject>(timesheetProjectId);
            item.IsActive = false;
        }
    }
}