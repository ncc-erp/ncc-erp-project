using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.PMReportProjectIssues.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReportProjectIssues
{
    [AbpAuthorize]
    public class PMReportProjectIssueAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize]
        public async Task<List<GetPMReportProjectIssueDto>> ProblemsOfTheWeek(long ProjectId, long pmReportId)
        {
            var query = from prpi in WorkScope.GetAll<PMReportProjectIssue>()
                        .Where(x => x.PMReportProject.ProjectId == ProjectId && x.PMReportProject.PMReportId == pmReportId)
                        .OrderByDescending(x => x.CreationTime)
                        select new GetPMReportProjectIssueDto
                        {
                            Id = prpi.Id,
                            PMReportProjectId = prpi.PMReportProjectId,
                            Description = prpi.Description,
                            Impact = prpi.Impact,
                            Critical = prpi.Critical.ToString(),
                            Source = prpi.Source.ToString(),
                            Solution = prpi.Solution,
                            MeetingSolution = prpi.MeetingSolution,
                            Status = prpi.Status.ToString(),
                            CreatedAt = prpi.CreationTime,
                        };
            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PMReportProjectIssueDto> Create(PMReportProjectIssueDto input, long projectId)
        {
            var pmReportProjectActive = await WorkScope.GetAll<PMReportProject>().Where(x => x.PMReport.IsActive && x.ProjectId == projectId).FirstOrDefaultAsync();
            if (pmReportProjectActive == null)
                throw new UserFriendlyException("Can't find any PMReportproject !");

            input.PMReportProjectId = pmReportProjectActive.Id;
            await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<PMReportProjectIssue>(input));
            return input;
        }

        [HttpPut]
        [AbpAuthorize]
        public async Task<PMReportProjectIssueDto> Update(PMReportProjectIssueDto input)
        {
            var pmReportProjectIssue = await WorkScope.GetAsync<PMReportProjectIssue>(input.Id);

            await WorkScope.UpdateAsync(ObjectMapper.Map<PMReportProjectIssueDto, PMReportProjectIssue>(input, pmReportProjectIssue));
            return input;
        } 

        [HttpDelete]
        [AbpAuthorize]
        public async Task Delete(long pmReportProjectIssueId)
        {
            await WorkScope.DeleteAsync<PMReportProjectIssue>(pmReportProjectIssueId);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.WeeklyReport_ReportDetail_PMIssue_AddMeetingNote)]
        public async Task<EditMeetingNoteDto> EditMeetingNote(EditMeetingNoteDto input)
        {
            var projectIssue = await WorkScope.GetAll<PMReportProjectIssue>()
                .Where(x => x.Id == input.Id)
                .FirstOrDefaultAsync();

            projectIssue.MeetingSolution = input.Note;
            await WorkScope.UpdateAsync(projectIssue);
            return input;
        }
    }
}
