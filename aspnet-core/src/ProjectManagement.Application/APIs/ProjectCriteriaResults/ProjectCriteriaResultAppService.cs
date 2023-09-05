using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using ProjectManagement.APIs.ProjectCriteriaResults.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectCriteriaResults
{
    public class ProjectCriteriaResultAppService : ProjectManagementAppServiceBase
    {
        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus
            )]
        [HttpPost]
        public async Task<CreateProjectCriteriaResultDto> Create(CreateProjectCriteriaResultDto input)
        {
            var projectWeeklyReportStatus = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.ProjectId == input.ProjectId && x.PMReportId == input.PMReportId).Select(x => x.Status).First();
            input.Id = await WorkScope.InsertAndGetIdAsync<ProjectCriteriaResult>(new ProjectCriteriaResult
            {
                Note = input.Note,
                PMReportId = input.PMReportId,
                Status = input.Status,
                ProjectCriteriaId = input.ProjectCriteriaId,
                ProjectId = input.ProjectId,
            });
            var listStatus = WorkScope.GetAll<ProjectCriteriaResult>()
                .Where(x => x.PMReportId == input.PMReportId && x.ProjectId == input.ProjectId && x.Id != input.Id)
                .Select(x => x.Status)
                .OrderByDescending(x => x)
                .ToList();

            if (projectWeeklyReportStatus == PMReportProjectStatus.Sent)
            {
                await SetProjectHealth(input, listStatus);
            }
            return input;
        }

        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus
            )]
        [HttpDelete]
        public async Task Delete(long projectCriteriaResultId)
        {
            var isExist = await WorkScope.GetAll<ProjectCriteriaResult>().AnyAsync(x => x.Id == projectCriteriaResultId);
            await WorkScope.DeleteAsync<ProjectCriteria>(projectCriteriaResultId);
        }

        [HttpGet]
        public async Task<GetProjectCriteriaResultDto> Get(long id)
        {
            var res = await WorkScope.GetAsync<ProjectCriteriaResult>(id);
            return new GetProjectCriteriaResultDto
            {
                CriteriaName = res.ProjectCriteria.Name,
                Guideline = res.ProjectCriteria.Guideline,
                Id = res.Id,
                Note = res.Note,
                PMReportId = res.PMReportId,
                ProjectId = res.ProjectId,
                Status = res.Status,
                ProjectCriteriaId = res.ProjectCriteriaId
            };
        }

        [AbpAuthorize(PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria)]
        [HttpGet]
        public async Task<List<GetProjectCriteriaResultDto>> GetAll(long pmReportId, long projectId)
        {
            var allowViewGuidelineProjectHealthCriteriaWRRD = await PermissionChecker.IsGrantedAsync(PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_View_Guideline);

            return await WorkScope.GetAll<ProjectCriteriaResult>().Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId)
               .Select(x => new GetProjectCriteriaResultDto
               {
                   CriteriaName = x.ProjectCriteria.Name,
                   Guideline = allowViewGuidelineProjectHealthCriteriaWRRD ? x.ProjectCriteria.Guideline : null,
                   Id = x.Id,
                   Note = x.Note,
                   PMReportId = x.PMReportId,
                   ProjectId = x.ProjectId,
                   Status = x.Status,
                   ProjectCriteriaId = x.ProjectCriteriaId
               }).ToListAsync();
        }

        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus
            )]
        [HttpPut]
        public async Task<CreateProjectCriteriaResultDto> Update(CreateProjectCriteriaResultDto input)
        {
            var prjCriteria = await WorkScope.GetAsync<ProjectCriteriaResult>(input.Id);
            var projectWeeklyReportStatus = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.ProjectId == input.ProjectId && x.PMReportId == input.PMReportId).Select(x => x.Status).First();
            await WorkScope.UpdateAsync(ObjectMapper.Map<CreateProjectCriteriaResultDto, ProjectCriteriaResult>(input, prjCriteria));
            var listStatus = WorkScope.GetAll<ProjectCriteriaResult>()
                .Where(x => x.PMReportId == input.PMReportId && x.ProjectId == input.ProjectId && x.Id != input.Id)
                .Select(x => x.Status)
                .OrderByDescending(x => x)
                .ToList();

            if (projectWeeklyReportStatus == PMReportProjectStatus.Sent)
            {
                await SetProjectHealth(input, listStatus);
            }
            return input;
        }

        private async Task UpdateHealth(long pmReport, long projectId, ProjectHealth health)
        {
            var item = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.PMReportId == pmReport && x.ProjectId == projectId).First();
            item.ProjectHealth = health;
            await WorkScope.UpdateAsync(item);
        }

        private async Task SetProjectHealth(CreateProjectCriteriaResultDto input, List<ProjectCriteriaResultStatus> listStatus)
        {
            var max = input.Status >= listStatus.First() ? input.Status : listStatus.First();
            await UpdateHealth(input.PMReportId, input.ProjectId, CommonUtil.GetProjectHealthByString(max.ToString()));
        }

        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_Edit,
            PermissionNames.WeeklyReport_ReportDetail_ProjectHealthCriteria_ChangeStatus
            )]
        [HttpPost]
        public async Task<List<CreateProjectCriteriaResultDto>> UpdateAllCriteriaResult(List<CreateProjectCriteriaResultDto> input)
        {
            var newItem = input.Where(x => x.Id <= 0 && (x.Note != "" || x.Status != 0)).Select(x => new ProjectCriteriaResult
            {
                Note = x.Note,
                PMReportId = x.PMReportId,
                Status = x.Status,
                ProjectCriteriaId = x.ProjectCriteriaId,
                ProjectId = x.ProjectId,
            });
            var updateItem = input.Where(x => x.Id > 0).Select(x => new ProjectCriteriaResult
            {
                Id = x.Id,
                Note = x.Note,
                PMReportId = x.PMReportId,
                Status = x.Status,
                ProjectCriteriaId = x.ProjectCriteriaId,
                ProjectId = x.ProjectId,
            });
            await WorkScope.UpdateRangeAsync(updateItem);
            var newItemDic = WorkScope.InsertRange(newItem).ToDictionary(x => x.ProjectCriteriaId);
            input.ForEach(x =>
            {
                if (newItemDic.Count > 0)
                {
                    x.Id = newItemDic.ContainsKey(x.ProjectCriteriaId) ? newItemDic[x.ProjectCriteriaId].Id : 0;
                }
            });
            var projectId = input.Select(x => x.ProjectId).First();
            var pmReportId = input.Select(x => x.PMReportId).First();
            var projectWeeklyReportStatus = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId).Select(x => x.Status).First();
            if (projectWeeklyReportStatus == PMReportProjectStatus.Sent)
            {
                var max = input.Max(x => x.Status);
                await UpdateHealth(pmReportId, projectId, CommonUtil.GetProjectHealthByString(max.ToString()));
            }
            return input;
        }
    }
}