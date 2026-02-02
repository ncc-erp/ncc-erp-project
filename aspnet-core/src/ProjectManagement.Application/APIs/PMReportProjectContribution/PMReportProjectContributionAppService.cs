using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.PMReportProjectContribution.Dto;
using ProjectManagement.APIs.PMReportProjectIssues.Dto;
using ProjectManagement.APIs.PMReports.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjectContribution
{
    [AbpAuthorize]
    public class PMReportProjectContributionAppService : ProjectManagementAppServiceBase
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        private readonly TimesheetService _timesheetService;

        private readonly ResourceManager _resourceManager;

        public PMReportProjectContributionAppService(IBackgroundJobManager backgroundJobManager, 
            TimesheetService timesheetService, 
            ResourceManager resourceManager)
        {
            _backgroundJobManager = backgroundJobManager;
            _timesheetService = timesheetService;
            _resourceManager = resourceManager;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<long> CreateOrUpdate(CreateOrEditWeeklyContributionDto input)
        {
            try
            {
                var resource = await WorkScope.GetAll<LinkedResource>()
                .FirstOrDefaultAsync(lr => lr.ProjectUserBillId == input.ProjectUserBillId && lr.UserId == input.UserId);

                if (resource == null)
                    throw new UserFriendlyException("Linked Resource not found");

                if (await CheckTotalContribute(input.ProjectUserBillId, input.Contribute, input.UserId))
                    throw new UserFriendlyException("Total contribution cannot exceed 100%.");

                resource.Contribute = input.Contribute;
                await WorkScope.UpdateAsync(resource);

                WeeklyContributionHistory history = null;

                if (input.Id.HasValue && input.Id > 0)
                {
                    history = await WorkScope.GetAsync<WeeklyContributionHistory>(input.Id.Value);
                }
                else
                {
                    history = await WorkScope.GetAll<WeeklyContributionHistory>()
                        .FirstOrDefaultAsync(x => x.PMReportId == input.PMReportId
                                               && x.UserId == input.UserId && x.ProjectUserBillId == input.ProjectUserBillId);
                }

                if (history != null)
                {
                    history.Contribute = input.Contribute;
                    await WorkScope.UpdateAsync(history);
                }
                else
                {

                    history = new WeeklyContributionHistory
                    {
                        ProjectUserBillId = input.ProjectUserBillId,
                        PMReportId = input.PMReportId,
                        Contribute = input.Contribute,
                        UserId = input.UserId,
                        ProjectId = input.ProjectId,
                        TenantId = AbpSession.TenantId
                    };
                    input.Id = await WorkScope.InsertAndGetIdAsync(history);
                }

                return history.Id;

            }
            catch (Exception ex)
            {
                Logger.Error("Error in CreateOrUpdate Weekly Contribution", ex);
                throw new UserFriendlyException("An error occurred while saving the contribution.");

            }
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<WeeklyContributionDto>> ContributionsThisWeek(long ProjectId, long pmReportId)
        {
            var query = from wc in WorkScope.GetAll<WeeklyContributionHistory>()
                        .Include(x => x.ProjectUserBill)
                        .ThenInclude(x => x.User)
                        .Where(x => x.ProjectId == ProjectId && x.PMReportId == pmReportId)
                        .OrderByDescending(x => x.CreationTime)
                        select new WeeklyContributionDto
                        {
                            Id = wc.Id,
                            Contribute = wc.Contribute,
                            PMReportId = wc.PMReportId,
                            PMReportName = wc.PMReport.Name,
                            ProjectId = wc.ProjectId,
                            ProjectName = wc.Project.Name,
                            ProjectUserBillId = wc.ProjectUserBillId,
                            UserId = wc.UserId,
                            UserName = wc.User.UserName,
                            UserFullName = wc.User.FullName
                        };
            return await query.ToListAsync();
        }

        #region API Helper
        private async Task<bool> CheckTotalContribute(long projectUserBillId, byte contribute, long userId)
        {
            var totalContribute = await WorkScope.GetAll<LinkedResource>()
                .Where(lr => lr.ProjectUserBillId == projectUserBillId && lr.UserId != userId)
                .SumAsync(lr => lr.Contribute);

            if (totalContribute + contribute > 100)
            {
                return true;
            }
            return false;
        }
        #endregion

    }
}
