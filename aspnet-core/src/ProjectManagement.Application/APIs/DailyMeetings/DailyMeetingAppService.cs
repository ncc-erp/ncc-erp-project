using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.DailyMeetings.Dto;
using ProjectManagement.APIs.Public.Dto;
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
                Criterias = summary.DailyReports.Select(d => new MeetingReportCriteriaDetailDto
                {
                    Id = d.Id,
                    CriteriaName = d.CriteriaName,
                    Content = d.Content,
                    //Status = d.Status,
                })
                .ToList()
            };
        }

        [AbpAuthorize]
        [HttpPut]
        public async Task UpdateSummary(UpdateSummaryDto input)
        {
            var item = await WorkScope.GetAsync<ProjectWeeklySummary>(input.Id);
            await WorkScope.UpdateAsync(item);
        }

        [AbpAuthorize]
        [HttpPut]
        public async Task UpdateMeetingReportCriteria(long id, UpdateMeetingReportCriteriaDto input)
        {
            var item = await WorkScope.GetAsync<MeetingReportCriteria>(id);
            item.CriteriaName = input.CriteriaName;
            item.Content = input.Content;
            //item.Status = input.Status;
            await WorkScope.UpdateAsync(item);
        }
        [AbpAuthorize]
        [HttpDelete]
        public async Task DeleteMeetingReportCriteria(long id)
        {
            await WorkScope.DeleteAsync<MeetingReportCriteria>(id);
        }

    }
}
