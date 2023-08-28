using Abp.Authorization;
using Abp.BackgroundJobs;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.PMReportProjectIssues.Dto;
using ProjectManagement.APIs.PMReportProjects.Dto;
using ProjectManagement.APIs.PMReports.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.Timesheet;
using ProjectManagement.Services.Timesheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReports
{
    [AbpAuthorize]
    public class PMReportAppService : ProjectManagementAppServiceBase
    {
        private readonly IBackgroundJobManager _backgroundJobManager;

        private readonly TimesheetService _timesheetService;

        private readonly ResourceManager _resourceManager;

        public PMReportAppService(IBackgroundJobManager backgroundJobManager, TimesheetService timesheetService, ResourceManager resourceManager)
        {
            _backgroundJobManager = backgroundJobManager;
            _timesheetService = timesheetService;
            _resourceManager = resourceManager;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.WeeklyReport)]
        public async Task<GridResult<GetPMReportDto>> GetAllPaging(GridParam input)
        {
            var pmReportProject = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.Project.IsRequiredWeeklyReport)
                .WhereIf(input.FilterItems.Any(x => x.PropertyName == "projectType" && int.Parse(x.Value.ToString()) == 2), x => x.Project.ProjectType == ProjectType.PRODUCT)
                .WhereIf(input.FilterItems.Any(x => x.PropertyName == "projectType" && int.Parse(x.Value.ToString()) == 1), x => x.Project.ProjectType == ProjectType.TRAINING)
                .WhereIf(input.FilterItems.Any(x => x.PropertyName == "projectType" && int.Parse(x.Value.ToString()) == 0) ||
                !input.FilterItems.Any(x => x.PropertyName == "projectType"),
                x => x.Project.ProjectType != ProjectType.TRAINING && x.Project.ProjectType != ProjectType.PRODUCT && x.Project.ProjectType != ProjectType.NoBill);
            var query = WorkScope.GetAll<PMReport>()
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new GetPMReportDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Year = x.Year,
                    IsActive = x.IsActive,
                    Type = x.Type,
                    PMReportStatus = x.PMReportStatus,
                    NumberOfProject = pmReportProject.Where(y => y.PMReportId == x.Id).Count(),
                    CountGreen = pmReportProject.Where(y => y.PMReportId == x.Id).Count(x => x.ProjectHealth == ProjectHealth.Green && x.Status == PMReportProjectStatus.Sent),
                    CountRed = pmReportProject.Where(y => y.PMReportId == x.Id).Count(x => x.ProjectHealth == ProjectHealth.Red && x.Status == PMReportProjectStatus.Sent),
                    CountYellow = pmReportProject.Where(y => y.PMReportId == x.Id).Count(x => x.ProjectHealth == ProjectHealth.Yellow && x.Status == PMReportProjectStatus.Sent),
                    CountDraft = pmReportProject.Where(y => y.PMReportId == x.Id).Count(x => x.Status == PMReportProjectStatus.Draft),
                    Note = x.Note,
                });
            input.FilterItems.RemoveAll(x => x.PropertyName == "projectType");
            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.WeeklyReport_CollectTimesheet)]
        public async Task<List<CollectTimesheetDto>> CollectTimesheet(long pmReportId, DateTime startTime, DateTime endTime)
        {
            var pmReportProjects = await WorkScope.GetAll<PMReportProject>()
                .Include(p => p.Project)
                .Include(p => p.Project.PM)
                .Where(x => x.PMReportId == pmReportId)
                .Select(s => new
                {
                    pmReportProject = s,
                    projectCode = s.Project.Code,
                    projectName = s.Project.Name,
                    pmName = s.Project.PM.Name + " " + s.Project.PM.Surname
                })
                .ToListAsync();

            var listProjectCode = pmReportProjects
                .Select(pro => pro.projectCode).ToList();

            var listTimsheetByProjectCode = await _timesheetService.GetTimesheetByListProjectCode(listProjectCode, startTime, endTime);

            Dictionary<String, TotalWorkingTimeOfWeekDto> mapProjectCodeToTimesheet = listTimsheetByProjectCode
                .GroupBy(s => s.ProjectCode)
                .ToDictionary(x => x.Key, s => s.FirstOrDefault());

            var result = new List<CollectTimesheetDto>();
            foreach (var pmReport in pmReportProjects)
            {
                TotalWorkingTimeOfWeekDto timesheet = mapProjectCodeToTimesheet.ContainsKey(pmReport.projectCode) ? mapProjectCodeToTimesheet[pmReport.projectCode] : default;
                if (timesheet != null)
                {
                    pmReport.pmReportProject.TotalNormalWorkingTime = timesheet.NormalWorkingTime;
                    pmReport.pmReportProject.TotalOverTime = timesheet.OverTime;
                    await WorkScope.UpdateAsync(pmReport.pmReportProject);
                    result.Add(new CollectTimesheetDto
                    {
                        NormalWorkingTime = timesheet.NormalWorkingTime,
                        OverTime = timesheet.OverTime,
                        ProjectCode = pmReport.projectCode,
                        ProjectName = pmReport.projectName,
                        PMName = pmReport.pmName,
                        Note = "Success"
                    });
                }
                else
                {
                    result.Add(new CollectTimesheetDto
                    {
                        ProjectCode = pmReport.projectCode,
                        ProjectName = pmReport.projectName,
                        PMName = pmReport.pmName,
                        Note = "Not found in Timesheet tool"
                    });
                }
            }

            return result;
        }

        public async Task<List<PMReportDto>> GetAll()
        {
            var query = WorkScope.GetAll<PMReport>()
                .OrderByDescending(x => x.IsActive)
                .OrderByDescending(x => x.CreationTime)
                .Select(x => new PMReportDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Year = x.Year,
                    IsActive = x.IsActive,
                    Type = x.Type,
                    PMReportStatus = x.PMReportStatus,
                    Note = x.Note,
                });
            return await query.ToListAsync();
        }

        public async Task<UpdateNoteDto> UpdateNote(UpdateNoteDto input)
        {
            var pmReport = await WorkScope.GetAsync<PMReport>(input.Id);
            //if (!pmReport.IsActive)
            //{
            //    throw new UserFriendlyException("Report has been closed !");
            //}
            pmReport.Note = input.Note;
            await WorkScope.UpdateAsync(pmReport);
            return input;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.WeeklyReport_CloseAndAddNew)]
        public async Task<CreatePMReportDto> Create(CreatePMReportDto input)
        {
            var isExist = await WorkScope.GetAll<PMReport>()
                .AnyAsync(x => x.Name == input.Name && x.Type == input.Type && x.Year == input.Year);

            if (isExist)
                throw new UserFriendlyException("PM Report already exist!");

            var activeReport = await WorkScope.GetAll<PMReport>().Where(x => x.IsActive).FirstOrDefaultAsync();
            long lastReportId = 0;
            if (activeReport != null)
            {
                activeReport.IsActive = false;
                await WorkScope.UpdateAsync(activeReport);
                lastReportId = activeReport.Id;
            }

            var pmReport = ObjectMapper.Map<PMReport>(input);
            input.Id = await WorkScope.InsertAndGetIdAsync(pmReport);
            pmReport.Id = input.Id;

            var userInFuture = WorkScope.GetAll<ProjectUser>()
                .Where(x => x.PMReportId == lastReportId && x.Status == ProjectUserStatus.Future);

            foreach (var pu in userInFuture)
            {
                pu.PMReportId = input.Id;
                pu.IsFutureActive = true;
                pu.Id = 0;
                await WorkScope.InsertAsync(pu);
            }

            var mapInprogressIssues = (await WorkScope.GetAll<PMReportProjectIssue>()
                .Where(x => x.Status == PMReportProjectIssueStatus.InProgress)
                .Where(s => s.PMReportProject.PMReportId == lastReportId)
                .Select(s => new
                {
                    s.PMReportProject.ProjectId,
                    Issues = s
                }).ToListAsync())
                .GroupBy(s => s.ProjectId)
                .ToDictionary(s => s.Key, s => s.Select(s => s.Issues).ToList());

            var mapImprogressRisks = (await WorkScope.GetAll<PMReportProjectRisk>()
                .Where(x => x.Status == PMReportProjectRiskStatus.InProgress)
                .Where(s => s.PMReportProject.PMReportId == lastReportId)
                .Select(s => new
                {
                    s.PMReportProject.ProjectId,
                    Risks = s
                }).ToListAsync())
                .GroupBy(s => s.ProjectId)
                .ToDictionary(s => s.Key, s => s.Select(s => s.Risks).ToList());

            var mapPMNote = (await WorkScope.GetAll<PMReportProject>()
                .Where(s => s.PMReportId == lastReportId)
                .Where(s => s.Project.ProjectType == ProjectType.TRAINING)
                .Select(s => new
                {
                    s.ProjectId,
                    Note = s.Note
                }).ToListAsync())
                .GroupBy(s => s.ProjectId)
                .ToDictionary(s => s.Key, s => s.Select(s => new { Note = s.Note }
                ).FirstOrDefault());

            var mapPMLastPreviewDate = (await WorkScope.GetAll<PMReportProject>()
                .Where(s => s.PMReportId == lastReportId)
                .Select(s => new
                {
                    s.ProjectId,
                    LastReviewDate = s.LastReviewDate
                }).ToListAsync())
                .GroupBy(s => s.ProjectId)
                .ToDictionary(s => s.Key, s => s.Select(s => new { LastReviewDate = s.LastReviewDate }
                ).FirstOrDefault());

            var activeProjects = await WorkScope.GetAll<Project>()
                .Where(x => x.Status == ProjectStatus.InProgress)
                .ToListAsync();

            foreach (var project in activeProjects)
            {
                var pmReportProject = new PMReportProject
                {
                    PMReportId = input.Id,
                    ProjectId = project.Id,
                    Status = PMReportProjectStatus.Draft,
                    //ProjectHealth = mapInprogressIssues.ContainsKey(project.Id) ? ProjectHealth.Yellow : ProjectHealth.Green,
                    PMId = project.PMId,
                    Note = project.ProjectType == ProjectType.TRAINING && mapPMNote.ContainsKey(project.Id) ? mapPMNote[project.Id].Note : null,
                    LastReviewDate = mapPMLastPreviewDate.ContainsKey(project.Id) ? mapPMLastPreviewDate[project.Id].LastReviewDate : null,
                };
                pmReportProject.Id = await WorkScope.InsertAndGetIdAsync(pmReportProject);

                if (mapInprogressIssues.ContainsKey(project.Id))
                {
                    var issues = mapInprogressIssues[project.Id];
                    if (issues != null && !issues.IsEmpty())
                    {
                        issues.ForEach(x =>
                        {
                            x.Id = 0;
                            x.PMReportProjectId = pmReportProject.Id;
                        });
                        await WorkScope.InsertRangeAsync(issues);
                    }
                }

                if (mapImprogressRisks.ContainsKey(project.Id))
                {
                    var risks = mapImprogressRisks[project.Id];
                    if (risks != null && !risks.IsEmpty())
                    {
                        risks.ForEach(x =>
                        {
                            x.Id = 0;
                            x.PMReportProjectId = pmReportProject.Id;
                        });
                        await WorkScope.InsertRangeAsync(risks);
                    }
                }
            }
            return input;
        }

        private DateTime SettingToDate(int day, int hour, DateTime timeline)
        {
            var result = timeline.AddDays(day - (int)timeline.DayOfWeek).AddHours(hour - timeline.Hour);
            return result;
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.WeeklyReport_Rename)]
        public async Task<PMReportDto> Update(PMReportDto input)
        {
            var pmReport = await WorkScope.GetAsync<PMReport>(input.Id);

            var isExist = await WorkScope.GetAll<PMReport>()
                .AnyAsync(x => x.Id != input.Id && x.Name == input.Name && x.Type == input.Type && x.Year == input.Year);
            if (isExist)
                throw new UserFriendlyException("PM Report already exist !");

            if (input.IsActive != pmReport.IsActive)
            {
                throw new UserFriendlyException("Report status cannot be edited !");
            }
            pmReport.Name = input.Name;

            await WorkScope.UpdateAsync(pmReport);
            return input;
        }

        [HttpDelete]
        public async Task Delete(long pmReportId)
        {
            var pmReport = await WorkScope.GetAsync<PMReport>(pmReportId);

            var hasPmReportProject = await WorkScope.GetAll<PMReportProject>().AnyAsync(x => x.PMReportId == pmReportId);
            if (hasPmReportProject)
                throw new UserFriendlyException("PM Report has PmReportProject !");

            await WorkScope.DeleteAsync(pmReport);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.WeeklyReport_CloseAndAddNew)]
        public async Task<string> CloseReport(long pmReportId)
        {
            var pmReport = await WorkScope.GetAsync<PMReport>(pmReportId);

            // phạt nếu chưa gửi report
            var pmReportProjects = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.PMReportId == pmReportId && x.Status == PMReportProjectStatus.Draft
                && x.PMReport.Type == PMReportType.Weekly);
            foreach (var i in pmReportProjects)
            {
                i.IsPunish = PunishStatus.High;
                await WorkScope.UpdateAsync(i);
            }

            var newPmReport = new CreatePMReportDto
            {
                Name = pmReport.Name + " (1)",
                Year = DateTime.Now.Year,
                IsActive = true,
                Type = PMReportType.Weekly
            };
            await Create(newPmReport);

            return $"{pmReport.Name} locked, new PmReport with name {pmReport.Name} (1) created";
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.WeeklyReport_View)]
        public async Task<ReportStatisticsDto> StatisticsReport(long pmReportId, DateTime startDate)
        {
            var changeInFuture = new List<TotalFutureUseDto>();

            var pmReport = await WorkScope.GetAsync<PMReport>(pmReportId);

            var issues = await (from p in WorkScope.GetAll<PMReportProjectIssue>().Where(x => x.PMReportProject.PMReportId == pmReportId)
                                select new GetPMReportProjectIssueDto
                                {
                                    Id = p.Id,
                                    PMReportProjectId = p.PMReportProjectId,
                                    ProjectName = p.PMReportProject.Project.Name,
                                    Description = p.Description,
                                    Impact = p.Impact,
                                    Critical = p.Critical.ToString(),
                                    Source = p.Source.ToString(),
                                    Solution = p.Solution,
                                    MeetingSolution = p.MeetingSolution,
                                    Status = p.Status.ToString(),
                                    ProjectHealth = p.PMReportProject.ProjectHealth,
                                    CreatedAt = p.CreationTime
                                }).ToListAsync();

            var projectUser = WorkScope.GetAll<ProjectUser>().Include(x => x.Project)
                .Where(x => x.Status != ProjectUserStatus.Past && x.IsFutureActive && x.Project.Status != ProjectStatus.Closed);
            var futureUser = projectUser.Where(x => x.StartTime.Date <= startDate.Date && x.Status == ProjectUserStatus.Future);
            var presentUser = projectUser.Where(x => x.Status == ProjectUserStatus.Present && x.IsFutureActive);

            var changeUse = from p in presentUser
                            join f in futureUser on p.UserId equals f.UserId
                            where p.ProjectId == f.ProjectId
                            select new
                            {
                                Present = p,
                                Future = f
                            };

            foreach (var item in changeUse)
            {
                if (item.Present.AllocatePercentage != item.Future.AllocatePercentage)
                {
                    changeInFuture.Add(new TotalFutureUseDto
                    {
                        UserId = item.Present.UserId,
                        ProjectId = item.Present.ProjectId,
                        Total = item.Future.AllocatePercentage - item.Present.AllocatePercentage
                    });
                }
            }

            var totalPercentageFuture = futureUser.Where(x => !changeInFuture.Select(r => r.UserId).Contains(x.Id) && !changeInFuture.Select(r => r.ProjectId).Contains(x.ProjectId));

            var query = from u in WorkScope.GetAll<User>().ToList()
                        join pu in projectUser on u.Id equals pu.UserId into pUser
                        join b in WorkScope.GetAll<ProjectManagement.Entities.Branch>() on u.BranchId equals b.Id into bUser
                        join c in changeInFuture on u.Id equals c.UserId into change
                        join t in totalPercentageFuture on u.Id equals t.UserId into newFuture
                        select new
                        {
                            UserId = u.Id,
                            FullName = u.Name + " " + u.Surname,
                            Avatar = u.AvatarPath,
                            UserType = u.UserType,
                            BranchColor = bUser.Select(x => x.Color).FirstOrDefault().ToString(),
                            BranchDislayName = bUser.Select(x => x.DisplayName).FirstOrDefault().ToString(),
                            UserEmail = u.EmailAddress,
                            TotalInTheWeek = pUser.Where(x => x.PMReportId == pmReportId && x.Status == ProjectUserStatus.Present).Sum(x => x.AllocatePercentage),
                            TotalInTheFuture = presentUser.Where(x => x.UserId == u.Id).Sum(x => x.AllocatePercentage) + change.Sum(x => x.Total) + newFuture.Sum(x => x.AllocatePercentage)
                        };

            var result = new ReportStatisticsDto
            {
                Note = pmReport.Note,
                Issues = issues,
                ResourceInTheWeek = query.OrderByDescending(x => x.TotalInTheWeek).Select(x => new ProjectUserStatistic
                {
                    UserId = x.UserId,
                    FullName = x.FullName,
                    Avatar = x.Avatar,
                    UserType = x.UserType,

                    //Branch = x.Branch,
                    BranchColor = x.BranchColor,
                    BranchDisplayName = x.BranchDislayName,
                    Email = x.UserEmail,
                    AllocatePercentage = x.TotalInTheWeek
                }).ToList(),
                ResourceInTheFuture = query.OrderByDescending(x => x.TotalInTheFuture).Select(x => new ProjectUserStatistic
                {
                    UserId = x.UserId,
                    FullName = x.FullName,
                    Avatar = x.Avatar,
                    UserType = x.UserType,

                    //Branch = x.Branch,
                    BranchColor = x.BranchColor,
                    BranchDisplayName = x.BranchDislayName,
                    Email = x.UserEmail,
                    AllocatePercentage = x.TotalInTheFuture
                }).ToList()
            };
            return result;
        }

        [AbpAuthorize]
        public async Task<PMReportDto> Get(long id)
        {
            var pmReport = await WorkScope.GetAsync<PMReport>(id);
            return new PMReportDto
            {
                Id = pmReport.Id,
                IsActive = pmReport.IsActive,
                Name = pmReport.Name,
                Note = pmReport.Note,
                PMReportStatus = pmReport.PMReportStatus,
                Type = pmReport.Type,
                Year = pmReport.Year
            };
        }
    }
}