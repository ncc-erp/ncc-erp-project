    using Abp.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using ProjectManagement.APIs.DailyMeetings.Dto;
    using ProjectManagement.Entities;
    using System.Linq;
    using System.Threading.Tasks;

    namespace ProjectManagement.APIs.DailyMeetings
    {
        public class DailyMeetingAppService : ProjectManagementAppServiceBase
        {
        [AbpAuthorize]
        [HttpGet]
        public async Task<GetProjectDailyMeetingsDto> Get(long projectId, long pmReportId)
        {
            var report = await WorkScope.GetAll<ProjectWeeklySummary>()
                .Include(x => x.DailyReports)
                .Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId)
                .FirstOrDefaultAsync();

            if (report == null) return null;

            return new GetProjectDailyMeetingsDto
            {
                Id = report.Id,
                ProjectId = report.ProjectId,
                PMReportId = report.PMReportId,
                Criterias = report.DailyReports
                    .Where(d => !d.IsDeleted)
                    .Select(d => new MeetingReportCriteriaDetailDto
                    {
                        Id = d.Id,
                        CriteriaName = d.CriteriaName,
                        Content = d.Content,
                        Status = d.Status
                    })
                    .ToList()
            };
        }

        [AbpAuthorize]
            [HttpPut]
            public async Task UpdateMeetingReportCriteria(long id, MeetingReportCriteriaDetailDto input)
            {
                var item = await WorkScope.GetAsync<MeetingReportCriteria>(id);
                item.CriteriaName = input.CriteriaName;
                item.Content = input.Content;
                item.Status = input.Status;
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
