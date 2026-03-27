using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.DailyMeetings.Dto;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.DailyMeetings
{
    public class DailyMeetingAppService : ProjectManagementAppServiceBase
    {
        [AbpAuthorize]
        [HttpGet]
        public async Task<GetProjectDailyMeetingsDto> Get(long projectId, long pmReportId)
        {
            var summary = await WorkScope.GetAll<ProjectWeeklySummary>()
                .Include(x => x.DailyReports)
                .Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId)
                .FirstOrDefaultAsync();

            if (summary == null)
            {
                return null;
            }

            return new GetProjectDailyMeetingsDto
            {
                Id = summary.Id,
                ProjectId = summary.ProjectId,
                PMReportId = summary.PMReportId,
                Summary = summary.OverallSummary,
                DailyReports = summary.DailyReports.Select(d => new ProjectDailyReportDto
                {
                    Id = d.Id,
                    Date = d.Date,
                    Content = d.Content
                })
                .OrderBy(d => d.Date)
                .ToList()
            };
        }

        [AbpAuthorize]
        [HttpPut]
        public async Task UpdateSummary(UpdateSummaryDto input)
        {
            var item = await WorkScope.GetAsync<ProjectWeeklySummary>(input.Id);
            item.OverallSummary = input.Summary;
            await WorkScope.UpdateAsync(item);
        }

        [AbpAuthorize]
        [HttpPut]
        public async Task UpdateDailyReport(UpdateDailyReportDto input)
        {
            var item = await WorkScope.GetAsync<ProjectDailyReport>(input.Id);
            item.Content = input.Content;
            await WorkScope.UpdateAsync(item);
        }
    }
}
