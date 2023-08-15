using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Uitls;
using ProjectManagement.APIs.PMReportProjectIssues.Dto;
using ProjectManagement.APIs.PMReportProjects.Dto;
using ProjectManagement.APIs.ProjectUsers.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Entities;
using ProjectManagement.NccCore.Helper;
using ProjectManagement.Services.PmReports;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Services.Timesheet;
using ProjectManagement.Services.Timesheet.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using PMUnsentWeeklyReportDto = ProjectManagement.Services.PmReports.Dto.PMUnsentWeeklyReportDto;

namespace ProjectManagement.APIs.PMReportProjects
{
    [AbpAuthorize]
    public class PMReportProjectAppService : ProjectManagementAppServiceBase
    {
        private readonly TimesheetService _timesheetService;

        private readonly ResourceManager _resourceManager;
        private readonly PmReportManager _pmReport;

        public PMReportProjectAppService(TimesheetService timesheetService,
            ResourceManager resourceManager, PmReportManager pmReport)
        {
            _timesheetService = timesheetService;
            _resourceManager = resourceManager;
            _pmReport = pmReport;
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.WeeklyReport_ReportDetail)]
        public async Task<List<GetPMReportProjectDto>> GetAllByPmReport(long pmReportId, WeeklyReportSort sort, PrioritizeReviewSort sortReview, ProjectHealth? health, string projectType = "OUTSOURCING")
        {
            var query = WorkScope.GetAll<PMReportProject>()
                .Where(x => x.PMReportId == pmReportId && x.Project.ProjectType != ProjectType.NoBill && x.Project.IsRequiredWeeklyReport)
                .WhereIf(health.HasValue, x => x.ProjectHealth == health.Value)
                .WhereIf(projectType == "PRODUCT", x => x.Project.ProjectType == ProjectType.PRODUCT)
                .WhereIf(projectType == "TRAINING", x => x.Project.ProjectType == ProjectType.TRAINING)
                .WhereIf(projectType == "OUTSOURCING", x => x.Project.ProjectType != ProjectType.TRAINING && x.Project.ProjectType != ProjectType.PRODUCT)
                .Select(x => new GetPMReportProjectDto
                {
                    Id = x.Id,
                    PMReportId = x.PMReportId,
                    PMReportName = x.PMReport.Name,
                    ProjectId = x.ProjectId,
                    ProjectName = x.Project.Name,
                    ProjectCode = x.Project.Code,
                    Status = x.Status.ToString(),
                    StatusEnum = x.Status,
                    ProjectHealthEnum = x.ProjectHealth,
                    PMId = x.PMId,
                    PmName = x.PM.Name,
                    Note = x.Note,
                    AutomationNote = x.AutomationNote,
                    PmBranch = x.PM.BranchOld,
                    PmEmailAddress = x.PM.EmailAddress,
                    PmAvatarPath = x.PM.AvatarPath,
                    PmFullName = x.PM.Name + " " + x.PM.Surname,
                    PmUserName = x.PM.UserName,
                    PmUserType = x.PM.UserType,
                    Seen = x.Seen,
                    TotalNormalWorkingTime = x.TotalNormalWorkingTime,
                    TotalOverTime = x.TotalOverTime,
                    LastReviewDate = x.LastReviewDate,
                    NecessaryReview = x.NecessaryReview,
                    IsActive = x.PMReport.IsActive,
                    ClientCode = x.Project.Client.Code,
                });

            switch (sort)
            {
                case WeeklyReportSort.No_Order:
                    //do nothing
                    query = query.OrderBy(q => true);
                    break;
                case WeeklyReportSort.Draft_Green_Yellow_Red:
                    query = query.OrderBy(x => x.StatusEnum).ThenBy(x => x.ProjectHealthEnum);
                    break;

                case WeeklyReportSort.Draft_Red_Yellow_Green:
                    query = query.OrderBy(x => x.StatusEnum).ThenByDescending(x => x.ProjectHealthEnum);
                    break;

                case WeeklyReportSort.Latest_Review_Last:
                    query = query.OrderBy(x => x.LastReviewDate);
                    break;
            }
            query = ((IOrderedQueryable<GetPMReportProjectDto>)query).ThenBy(q => q.PmEmailAddress).ThenBy(p => p.ProjectName);
            switch (sortReview)
            {
                case PrioritizeReviewSort.All:
                    // do nothing
                    break;

                case PrioritizeReviewSort.Nothing:
                    query = query.Where(x => x.NecessaryReview == false && x.Seen == false);
                    break;

                case PrioritizeReviewSort.Need_Review:
                    query = query.Where(x => x.NecessaryReview == true);
                    break;

                case PrioritizeReviewSort.Reviewed:
                    query = query.Where(x => x.Seen == true);
                    break;
            }
            var list = (query.AsEnumerable())
                .Select(x => { x.PmEmailAddress = UserHelper.GetUserName(x.PmEmailAddress); return x; });
            return list.ToList();
        }

        private List<GetPMReportProjectDto> shuffleList(List<GetPMReportProjectDto> list)
        {
            if (list == null || list.IsEmpty())
            {
                return null;
            }
            var rnd = new Random();
            var dicPMIdToRandomValue = list.Select(s => s.PMId)
                .Distinct()
                .ToDictionary(k => k, v => rnd.Next());

            return list.OrderBy(s => dicPMIdToRandomValue[s.PMId]).ToList();
        }

        [HttpGet]
        public async Task<object> GetInfoProject(long pmReportProjectId)
        {
            var projectUser = WorkScope.GetAll<ProjectUser>()
                .Where(x => x.User.UserType != UserType.FakeUser)
                .Where(x => x.Status == ProjectUserStatus.Present && x.AllocatePercentage > 0);
            var projectUserBill = WorkScope.GetAll<ProjectUserBill>()
                .Where(x => x.User.IsDeleted == false)
                .Where(x => x.isActive);

            var query = WorkScope.GetAll<PMReportProject>().Where(x => x.Id == pmReportProjectId)
                                        .Select(x => new
                                        {
                                            ProjectCode = x.Project.Code,
                                            ProjectName = x.Project.Name,
                                            ClientName = x.Project.Client.Name,
                                            ClientCode = x.Project.Client.Code,
                                            PmName = x.PM.FullName,
                                            TotalBill = projectUserBill.Where(b => b.ProjectId == x.ProjectId).Count(),
                                            TotalResource = projectUser.Where(r => r.ProjectId == x.ProjectId).Count(),
                                            TotalNormalWorkingTime = x.TotalNormalWorkingTime,
                                            TotalOverTime = x.TotalOverTime,
                                            PmNote = x.Note,
                                            AutomationNote = x.AutomationNote
                                        });

            return await query.FirstOrDefaultAsync();
        }

        [HttpGet]
        public async Task<List<UserOfProjectDto>> GetWorkingResourceOfProject(long projectId)
        {
            //var totalPercent = from pu in WorkScope.GetAll<ProjectUser>().Where(x => x.Project.Status != ProjectStatus.Closed)
            //                   .Where(x => x.Status == ProjectUserStatus.Present )
            //                   select new
            //                   {
            //                       UserId = pu.UserId,
            //                       TotalPercent = pu.AllocatePercentage
            //                   };

            //var projectUsers = WorkScope.GetAll<ProjectUser>()
            //                    .Where(x => x.ProjectId == projectId)
            //                    .Where(x => x.Status == ProjectUserStatus.Present && x.AllocatePercentage > 0)
            //                    .Where(x => x.User.UserType != UserType.FakeUser)
            //                    .Select(x => new CurrentResourceDto
            //                    {
            //                        UserId= x.UserId,
            //                        FullName = x.User.FullName,
            //                        ProjectRole = x.ProjectRole.ToString(),
            //                        AllocatePercentage = x.AllocatePercentage,
            //                        TotalPercent = totalPercent.Where(t => t.UserId == x.UserId).Sum(x => x.TotalPercent)
            //                    });
            //return await projectUsers.ToListAsync();
            var query = _resourceManager.QueryUsersOfProject(projectId)
                .Where(x => x.PUStatus == ProjectUserStatus.Present && x.AllocatePercentage > 0);

            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<TotalWorkingTimeOfWeekDto> GetWorkingTimeFromTimesheet(long pmReportProjectId, DateTime startTime, DateTime endTime)
        {
            var pmReportProject = await WorkScope.GetAll<PMReportProject>()
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.Id == pmReportProjectId);

            var result = await _timesheetService.GetWorkingHourFromTimesheet(pmReportProject.Project.Code, startTime, endTime);

            pmReportProject.TotalNormalWorkingTime = result.NormalWorkingTime;
            pmReportProject.TotalOverTime = result.OverTime;

            await WorkScope.UpdateAsync(pmReportProject);

            return new TotalWorkingTimeOfWeekDto
            {
                NormalWorkingTime = result.NormalWorkingTime,
                OverTime = result.OverTime
            };
        }

        [HttpGet]
        public async Task<GetResultpmReportProjectIssue> ProblemsOfTheWeekForReport(long ProjectId, long pmReportId)
        {
            var pmReportProject = await WorkScope.GetAll<PMReportProject>().Where(x => x.ProjectId == ProjectId && x.PMReportId == pmReportId).FirstOrDefaultAsync();
            if (pmReportProject == null)
            {
                throw new UserFriendlyException("This project doesn't have report for this week");
            }
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
                            TotalWeekAgo = (DateTimeUtils.GetNow() - prpi.CreationTime).Days,
                        };

            var result = new GetResultpmReportProjectIssue
            {
                PmReportProjectId = pmReportProject.Id,
                ProjectHealth = pmReportProject.ProjectHealth,
                Status = Enum.GetName(typeof(PMReportProjectStatus), pmReportProject.Status),
                ProjectHealthString = pmReportProject.ProjectHealth == 0 ? "Grey" : Enum.GetName(typeof(ProjectHealth), pmReportProject.ProjectHealth),
                TimeSendReport = pmReportProject.TimeSendReport,
                Result = query.ToList()
            };

            return result;
        }

        //[HttpGet]
        //[AbpAuthorize(PermissionNames.WeeklyReport_ReportDetail_UpdateProjectHealth,
        //    PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth,
        //    PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth,
        //    PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateProjectHealth)
        //]
        //public async Task UpdateHealth(long pmReportProjectId, ProjectHealth projectHealth)
        //{
        //    var pmReportProject = await WorkScope.GetAll<PMReportProject>().Include(x => x.PMReport)
        //                                .Where(x => x.Id == pmReportProjectId).FirstOrDefaultAsync();

        //    if (!pmReportProject.PMReport.IsActive)
        //    {
        //        throw new UserFriendlyException("Report has been closed !");
        //    }

        //    pmReportProject.ProjectHealth = projectHealth;
        //    await WorkScope.UpdateAsync(pmReportProject);
        //}

        [HttpGet]
        public async Task SetDoneIssue(long id)
        {
            var pmReportProject = await WorkScope.GetAll<PMReportProjectIssue>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (pmReportProject.Status == PMReportProjectIssueStatus.InProgress)
            {
                pmReportProject.Status = PMReportProjectIssueStatus.Done;
            }
            else
            {
                pmReportProject.Status = PMReportProjectIssueStatus.InProgress;
            }
            await WorkScope.UpdateAsync(pmReportProject);
        }

        [HttpGet]
        public async Task<List<GetPMReportProjectDto>> GetAllPmReportProjectForDropDown()
        {
            var query = WorkScope.GetAll<PMReportProject>().Include(x => x.PM)
                              .Where(x => x.PMReport.IsActive)
                              .Select(x => new GetPMReportProjectDto
                              {
                                  Id = x.Id,
                                  PMReportId = x.PMReportId,
                                  PMReportName = x.PMReport.Name,
                                  ProjectId = x.ProjectId,
                                  ProjectName = x.Project.Name,
                                  Status = x.Status.ToString(),
                                  ProjectHealthEnum = x.ProjectHealth,
                                  PMId = x.PMId,
                                  Note = x.Note,
                                  PmName = x.PM.Name,
                                  PmAvatarPath = x.PM.AvatarPath,
                                  PmEmailAddress = x.PM.EmailAddress,
                                  PmFullName = x.PM.FullName,
                                  PmUserName = x.PM.UserName,
                                  PmBranch = x.PM.BranchOld,
                                  PmUserType = x.PM.UserType,
                                  NecessaryReview = x.NecessaryReview
                              });

            return await query.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.WeeklyReport_ReportDetail_ChangedResource,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_ChangedResource,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_ChangedResource,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_ChangedResource)
        ]
        public async Task<List<GetProjectUserDto>> ResourceChangesDuringTheWeek(long projectId, long pmReportId)
        {
            var query = WorkScope.GetAll<ProjectUser>().Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId)
                            .Where(x => x.Status == ProjectUserStatus.Present).OrderByDescending(x => x.CreationTime)
                            .Where(x => x.User.UserType != UserType.FakeUser)
                            .Select(x => new GetProjectUserDto
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                FullName = x.User.FullName,
                                ProjectId = x.ProjectId,
                                ProjectName = x.Project.Name,
                                ProjectRole = x.ProjectRole.ToString(),
                                AllocatePercentage = x.AllocatePercentage,
                                StartTime = x.StartTime.Date,
                                Status = x.Status.ToString(),
                                IsExpense = x.IsExpense,
                                ResourceRequestId = x.ResourceRequestId,
                                ResourceRequestName = x.ResourceRequest.Name,
                                PMReportId = x.PMReportId,
                                PMReportName = x.PMReport.Name,
                                IsFutureActive = x.IsFutureActive,
                                AvatarPath = x.User.AvatarPath,
                                EmailAddress = x.User.EmailAddress,
                                UserName = x.User.UserName,
                                Branch = x.User.BranchOld,
                                UserType = x.User.UserType,
                                Note = x.Note
                            });
            return await query.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.WeeklyReport_ReportDetail_PlannedResource,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_PlannedResource,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_PlannedResource,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_PlannedResource)
        ]
        public async Task<List<GetProjectUserDto>> ResourceChangesInTheFuture(long projectId, long pmReportId)
        {
            var query = WorkScope.GetAll<ProjectUser>().Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId)
                            .Where(x => x.Status == ProjectUserStatus.Future).OrderByDescending(x => x.CreationTime)
                            .Select(x => new GetProjectUserDto
                            {
                                Id = x.Id,
                                UserId = x.UserId,
                                FullName = x.User.FullName,
                                ProjectId = x.ProjectId,
                                ProjectName = x.Project.Name,
                                ProjectRole = x.ProjectRole.ToString(),
                                AllocatePercentage = x.AllocatePercentage,
                                StartTime = x.StartTime.Date,
                                Status = x.Status.ToString(),
                                IsExpense = x.IsExpense,
                                ResourceRequestId = x.ResourceRequestId,
                                ResourceRequestName = x.ResourceRequest.Name,
                                PMReportId = x.PMReportId,
                                PMReportName = x.PMReport.Name,
                                IsFutureActive = x.IsFutureActive,
                                UserName = x.User.UserName,
                                AvatarPath = x.User.AvatarPath,
                                EmailAddress = x.User.EmailAddress,
                                UserType = x.User.UserType,
                                Branch = x.User.BranchOld,
                                Note = x.Note
                            });
            query = query.Where(x => x.UserType != UserType.FakeUser);
            return await query.ToListAsync();
        }

        public async Task<List<UserOfProjectDto>> GetPlannedResourceByPmReport(long projectId, long pmReportId)
        {
            var query = _resourceManager.QueryPlansOfProject(projectId)
                .Where(s => s.PMReportId == pmReportId);
            return await query.Where(x => x.PMReportId == pmReportId).ToListAsync();
        }

        public async Task<List<UserOfProjectDto>> GetchangedResourceByPmReport(long projectId, long pmReportId)
        {
            var query = _resourceManager.QueryUsersOfProject(projectId);
            return await query.Where(x => x.PMReportId == pmReportId)
                             .Where(x => x.ProjectId == projectId)
                             .Where(x => x.PUStatus != ProjectUserStatus.Future)
                             .OrderByDescending(x => x.StartTime)
                             .ToListAsync();
        }

        [HttpDelete]
        [AbpAuthorize()]
        public async Task CancelResourcePlan(long projectUserId)
        {
            await _resourceManager.DeleteFuturePUAndNotify(projectUserId);
        }

        [HttpPost]
        [AbpAuthorize()]
        public async Task ConfirmOutProject(ConfirmOutProjectDto input)
        {
            await _resourceManager.ConfirmOutProject(input);
        }

        [HttpGet]
        [AbpAuthorize()]
        public async Task ConfirmJoinProject(long projectUserId, DateTime startTime)
        {
            var allowConfirmMoveEmployeeToOtherProject
            = await PermissionChecker.IsGrantedAsync(PermissionNames.WeeklyReport_ReportDetail_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther);
            await _resourceManager.ConfirmJoinProject(projectUserId, startTime, allowConfirmMoveEmployeeToOtherProject);
        }

        [HttpPost]
        [AbpAuthorize()]
        public async Task EditProjectUserPlan(EditProjectUserDto input)
        {
            await _resourceManager.EditProjectUserPlan(input);
        }

        [HttpPost]
        public async Task PlanEmployeeJoinProject(InputPlanResourceDto input)
        {
            await _resourceManager.AddFuturePU(input);
        }

        [HttpPost]
        [AbpAuthorize(
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_SendWeeklyReport)
        ]
        public async Task SendReport(long projectId, long pmReportId, string status)
        {
            var pmReportProject = await WorkScope.GetAll<PMReportProject>().Include(x => x.PMReport)
                .Where(x => x.ProjectId == projectId && x.PMReportId == pmReportId).FirstOrDefaultAsync();
            if (pmReportProject.Status == PMReportProjectStatus.Sent)
                throw new UserFriendlyException("Report has been sent !");

            pmReportProject.Status = PMReportProjectStatus.Sent;
            pmReportProject.TimeSendReport = DateTimeUtils.GetNow();
            pmReportProject.ProjectHealth = CommonUtil.GetProjectHealthByString(status);
            // phạt nhẹ nếu quá hạn
            if (pmReportProject.PMReport.PMReportStatus == PMReportStatus.Expired && pmReportProject.PMReport.Type == PMReportType.Weekly)
            {
                pmReportProject.IsPunish = PunishStatus.Low;
            }

            await WorkScope.UpdateAsync(pmReportProject);
        }

        [HttpPost]
        public async Task<PMReportProjectDto> Create(PMReportProjectDto input)
        {
            var pmReport = await WorkScope.GetAsync<PMReport>(input.PMReportId);
            if (!pmReport.IsActive)
            {
                throw new UserFriendlyException("PMReport is locked !");
            }

            var isExist = await WorkScope.GetAll<PMReportProject>().AnyAsync(x => x.PMReportId == input.PMReportId && x.ProjectId == input.ProjectId);
            if (isExist)
                throw new UserFriendlyException("PMReportProject already exist !");

            input.Status = PMReportProjectStatus.Draft;
            await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<PMReportProject>(input));
            return input;
        }

        [HttpPut]
        public async Task<PMReportProjectDto> Update(PMReportProjectDto input)
        {
            var pmReportProject = await WorkScope.GetAsync<PMReportProject>(input.Id);

            if (!pmReportProject.PMReport.IsActive)
            {
                throw new UserFriendlyException("PMReport is locked !");
            }

            var isExist = await WorkScope.GetAll<PMReportProject>().AnyAsync(x => x.Id != input.Id && x.PMReportId == input.PMReportId && x.ProjectId == input.ProjectId);
            if (isExist)
                throw new UserFriendlyException("PMReportProject already exist !");

            await WorkScope.UpdateAsync(ObjectMapper.Map<PMReportProjectDto, PMReportProject>(input, pmReportProject));
            return input;
        }

        [HttpDelete]
        public async Task Delete(long pmPeportProjectId)
        {
            var pmReportProject = await WorkScope.GetAsync<PMReportProject>(pmPeportProjectId);
            var pmReport = await WorkScope.GetAsync<PMReport>(pmReportProject.PMReportId);
            if (!pmReport.IsActive)
            {
                throw new UserFriendlyException("PMReport is locked !");
            }

            await WorkScope.DeleteAsync<PMReportProject>(pmPeportProjectId);
        }

        public async Task<DateTime?> ReverseSeen(long pmReportProjectId)
        {
            var pmReportProject = await WorkScope.GetAsync<PMReportProject>(pmReportProjectId);
            if (pmReportProject.Seen != true)
            {
                pmReportProject.LastReviewDate = DateTime.Now;
            }
            pmReportProject.Seen = !pmReportProject.Seen;
            await WorkScope.UpdateAsync(pmReportProject);
            return pmReportProject.LastReviewDate;
        }

        public async Task<bool?> CheckNecessaryReview(long pmReportProjectId)
        {
            var pmReportProject = await WorkScope.GetAsync<PMReportProject>(pmReportProjectId);
            pmReportProject.NecessaryReview = !pmReportProject.NecessaryReview;
            await WorkScope.UpdateAsync(pmReportProject);
            return pmReportProject.NecessaryReview;
        }

        [HttpGet]
        public async Task<List<GetAllByProjectDto>> GetAllByProject(long projectId)
        {
            var query = from p in WorkScope.GetAll<PMReport>()
                        join pp in WorkScope.GetAll<PMReportProject>().Where(x => x.ProjectId == projectId)
                        on p.Id equals pp.PMReportId into lst
                        from l in lst.DefaultIfEmpty()
                        orderby p.IsActive descending
                        orderby p.CreationTime descending
                        select new GetAllByProjectDto
                        {
                            ReportId = p.Id,
                            PmReportProjectId = l.Id,
                            PMReportName = p.Name,
                            Status = l.Status.ToString(),
                            IsActive = p.IsActive,
                            Note = l.Note,
                            ProjectHealth = l.ProjectHealth.ToString()
                        };

            return await query.ToListAsync();
        }

        [AbpAuthorize(
              PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateNote,
              PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote,
              PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateNote,
              PermissionNames.WeeklyReport_ReportDetail_UpdateNote)
          ]
        public async Task<UpdateNoteDto> UpdateNote(UpdateNoteDto input)
        {
            var pmReportProject = await WorkScope.GetAll<PMReportProject>().Include(x => x.PMReport).SingleOrDefaultAsync(x => x.Id == input.Id);

            if (!pmReportProject.PMReport.IsActive)
            {
                throw new UserFriendlyException("Report has been closed !");
            }

            pmReportProject.Note = input.Note;
            await WorkScope.UpdateAsync(pmReportProject);
            return input;
        }

        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabWeeklyReport_UpdateNote,
              PermissionNames.Projects_ProductProjects_ProjectDetail_TabWeeklyReport_UpdateNote,
              PermissionNames.Projects_TrainingProjects_ProjectDetail_TabWeeklyReport_UpdateNote,
              PermissionNames.WeeklyReport_ReportDetail_UpdateNote)]
        public async Task<UpdateNoteDto> UpdateAutomationNote(UpdateNoteDto input)
        {
            var pmReportProject = await WorkScope.GetAll<PMReportProject>()
                .Include(x => x.PMReport)
                .SingleOrDefaultAsync(x => x.Id == input.Id);

            if (!pmReportProject.PMReport.IsActive)
            {
                throw new UserFriendlyException("Report has been closed !");
            }

            pmReportProject.AutomationNote = input.Note;
            await WorkScope.UpdateAsync(pmReportProject);
            return input;
        }

        // get all pm unsent weekly report
        [HttpGet]
        [AbpAuthorize()]
        public async Task<List<PMUnsentWeeklyReportDto>> GetPMsUnSentWeeklyReport()
        {
            return await _pmReport.GetPMsUnsentWeeklyReport();
        }

    }
}