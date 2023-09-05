using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using ProjectManagement.APIs.PMReportProjectRisks.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.PMReportProjectRisks
{
    [AbpAuthorize]
    public class PMReportProjectRiskAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_View,
            PermissionNames.WeeklyReport_ReportDetail_PMRisk_View)]
        public async Task<List<GetPMReportProjectRiskDto>> RisksOfTheWeek(long ProjectId, long pmReportId)
        {
            var query = WorkScope.GetAll<PMReportProjectRisk>()
                        .Where(x => x.PMReportProject.ProjectId == ProjectId && x.PMReportProject.PMReportId == pmReportId)
                        .OrderByDescending(x => x.CreationTime)
                        .Select(prpi => new GetPMReportProjectRiskDto
                        {
                            Id = prpi.Id,
                            PMReportProjectId = prpi.PMReportProjectId,
                            Risk = prpi.Risk,
                            Impact = prpi.Impact,
                            Solution = prpi.Solution,
                            Status = prpi.Status,
                            Priority = prpi.Priority,
                            CreatedAt = prpi.CreationTime,
                        }).ToListAsync();
            return await query;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_AddNewRisk)]
        public async Task<PMReportProjectRiskDto> Create(PMReportProjectRiskDto input, long projectId)
        {

            var pmReportProjectActive = await WorkScope.GetAll<PMReportProject>().Where(x => x.PMReport.IsActive && x.ProjectId == projectId).FirstOrDefaultAsync();
            if (pmReportProjectActive == null)
                throw new UserFriendlyException("Can't find any PMReportProject !");

            input.PMReportProjectId = pmReportProjectActive.Id;
            await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<PMReportProjectRisk>(input));
            return input;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Edit,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_SetDone,
            PermissionNames.WeeklyReport_ReportDetail_PMRisk_SetDone)
            ]
        public async Task<PMReportProjectRiskDto> Update(PMReportProjectRiskDto input)
        {
            var pmReportProjectIssue = await WorkScope.GetAsync<PMReportProjectRisk>(input.Id);

            await WorkScope.UpdateAsync(ObjectMapper.Map<PMReportProjectRiskDto, PMReportProjectRisk>(input, pmReportProjectIssue));
            return input;
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PMProjectRisk_Delete)]
        public async Task Delete(long pmReportProjectIssueId)
        {
            await WorkScope.DeleteAsync<PMReportProjectRisk>(pmReportProjectIssueId);
        }
    }
}