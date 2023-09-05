using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.Projects.Dto;
using ProjectManagement.APIs.ProjectUsers;
using ProjectManagement.APIs.Technologys.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Roles;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.Timesheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Projects
{
    [AbpAuthorize]
    public class ProjectAppService : ProjectManagementAppServiceBase
    {
        private readonly IProjectUserAppService _projectUserAppService;
        private readonly ResourceManager _resourceManager;
        private readonly TimesheetService _timesheetService;
        private readonly ISettingManager _settingManager;
        private readonly IRepository<UserRole, long> _userRoleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProjectAppService(IProjectUserAppService projectUserAppService,
            TimesheetService timesheetService,
            IRepository<UserRole, long> userRoleRepository,
            ResourceManager resourceManager,
            ISettingManager settingManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _projectUserAppService = projectUserAppService;
            _timesheetService = timesheetService;
            _userRoleRepository = userRoleRepository;
            _resourceManager = resourceManager;
            _settingManager = settingManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ViewAllProject, PermissionNames.Projects_OutsourcingProjects_ViewMyProjectOnly)]
        public async Task<GridResult<GetProjectDto>> GetAllPaging(GridParam input)
        {
            bool isViewAll = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ViewAllProject);
            bool hasViewBillPermission = PermissionChecker.IsGranted(PermissionNames.Projects_OutsourcingProjects_ViewBillInfo);
            bool hasViewBillAccountPermission = PermissionChecker.IsGranted(PermissionNames.Projects_OutsourcingProjects_ViewBillAccount);
            bool hasViewRequireWRPermission = PermissionChecker.IsGranted(PermissionNames.Projects_OutsourcingProjects_ViewRequireWeeklyReport);

            var filterStatus = input.FilterItems != null ? input.FilterItems.FirstOrDefault(x => x.PropertyName == "status") : null;
            var filterPmId = input.FilterItems != null ? input.FilterItems.FirstOrDefault(x => x.PropertyName == "pmId" && Convert.ToInt64(x.Value) == -1) : null;
            int valueStatus = -1;
            if (filterStatus != null)
            {
                valueStatus = Convert.ToInt32(filterStatus.Value);
                input.FilterItems.Remove(filterStatus);
            }
            if (filterPmId != null)
            {
                input.FilterItems.Remove(filterPmId);
            }
            var query = from p in WorkScope.GetAll<Project>().Include(x => x.Currency)
                        .Where(x => isViewAll || x.PMId == AbpSession.UserId.Value)
                        .Where(x => x.ProjectType != ProjectType.TRAINING && x.ProjectType != ProjectType.PRODUCT)
                        .Where(x => filterStatus != null && valueStatus > -1 ? (valueStatus == 3 ? x.Status != ProjectStatus.Closed : x.Status == (ProjectStatus)valueStatus) : true)
                        join rp in WorkScope.GetAll<PMReportProject>().Where(x => x.PMReport.IsActive) on p.Id equals rp.ProjectId into lst
                        from l in lst.DefaultIfEmpty()
                        select new GetProjectDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Code = p.Code,
                            ProjectType = p.ProjectType,
                            StartTime = p.StartTime.Date,
                            EndTime = p.EndTime.Value.Date,
                            Status = p.Status,
                            ClientId = p.ClientId,
                            ClientName = p.Client.Name,
                            ClientCode = p.Client.Code,
                            CurrencyId = p.CurrencyId,
                            CurrencyName = p.Currency.Name,
                            IsCharge = p.IsCharge,
                            ChargeType = p.ChargeType,
                            PmId = p.PMId,
                            PmName = p.PM.Name,
                            PmFullName = p.PM.FullName,
                            PmAvatarPath = p.PM.AvatarPath,
                            PmEmailAddress = p.PM.EmailAddress,
                            PmUserName = p.PM.UserName,
                            PmUserType = p.PM.UserType,
                            PmUserLevel = p.PM.UserLevel,
                            PmBranch = p.PM.BranchOld,
                            PmBranchColor = p.PM.Branch.Color,
                            PmBranchDisplayName = p.PM.Branch.DisplayName,
                            PositionId = p.PM.PositionId,
                            PositionName = p.PM.Position.ShortName,
                            PositionColor = p.PM.Position.Color,
                            IsSent = l.Status,
                            TimeSendReport = l.TimeSendReport,
                            DateSendReport = l.TimeSendReport.Value.Date,
                            RequireTimesheetFile = p.RequireTimesheetFile,
                            IsRequiredWeeklyReport = hasViewRequireWRPermission ? p.IsRequiredWeeklyReport : default(bool?),
                            BillInfo = hasViewBillPermission || hasViewBillAccountPermission ? WorkScope.GetAll<ProjectUserBill>()
                            .Where(b => b.ProjectId == p.Id)
                            .Where(b => b.isActive)
                            .OrderByDescending(b => b.CreationTime)
                                    .Select(b => new GetBillInfoDto
                                    {
                                        BillRole = b.BillRole,
                                        BillRate = hasViewBillPermission ? b.BillRate : float.NaN,
                                        StartTime = b.StartTime,
                                        EndTime = b.EndTime.Value,
                                        isActive = b.isActive,
                                        FullName = b.User.FullName,
                                    }).ToList() : null
                        };
            return await query.GetGridResult(query, input);
        }

        public async Task<IActionResult> GetOutsourcingPMs()
        {
            var pms = await WorkScope.GetAll<Project>()
                .Where(u => u.ProjectType != ProjectType.PRODUCT && u.ProjectType != ProjectType.TRAINING)
                .Select(u => new PMDto
                {
                    Id = u.PMId,
                    EmailAddress = u.PM.EmailAddress,
                    FullName = u.PM.FullName,
                    AvatarPath = u.PM.AvatarPath,
                    UserType = u.PM.UserType,
                    UserLevel = u.PM.UserLevel,
                    Branch = u.PM.BranchOld,
                })
                .Distinct()
                .ToListAsync();
            return new OkObjectResult(pms);
        }

        public async Task<IActionResult> GetProductPMs()
        {
            var pms = await WorkScope.GetAll<Project>()
                .Where(u => u.ProjectType == ProjectType.PRODUCT)
                .Select(u => new PMDto
                {
                    Id = u.PMId,
                    EmailAddress = u.PM.EmailAddress,
                    FullName = u.PM.FullName,
                    AvatarPath = u.PM.AvatarPath,
                    UserType = u.PM.UserType,
                    UserLevel = u.PM.UserLevel,
                    Branch = u.PM.BranchOld,
                })
                .Distinct()
                .ToListAsync();
            return new OkObjectResult(pms);
        }

        public async Task<IActionResult> GetTrainingPMs()
        {
            var pms = await WorkScope.GetAll<Project>()
                .Where(u => u.ProjectType == ProjectType.TRAINING)
                .Select(u => new PMDto
                {
                    Id = u.PMId,
                    EmailAddress = u.PM.EmailAddress,
                    FullName = u.PM.FullName,
                    AvatarPath = u.PM.AvatarPath,
                    UserType = u.PM.UserType,
                    UserLevel = u.PM.UserLevel,
                    Branch = u.PM.BranchOld,
                })
                .Distinct()
                .ToListAsync();
            return new OkObjectResult(pms);
        }

        [HttpGet]
        public async Task<List<ProjectInfoDto>> GetAllProjectInfo()
        {
            var query = WorkScope.GetAll<Project>()
                .Select(x => new ProjectInfoDto
                {
                    ProjectId = x.Id,
                    ProjectName = x.Name,
                    ProjectType = x.ProjectType,
                    ProjectStatus = x.Status,
                });
            return await query.ToListAsync();
        }

        [HttpGet]
        public async Task<List<GetProjectDto>> GetAll(bool isfilter = false)
        {
            var query = WorkScope.GetAll<Project>()
                .Include(x => x.Currency)
                .WhereIf(isfilter = true, x => x.Status != ProjectStatus.Closed)
                .Select(x => new GetProjectDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ProjectType = x.ProjectType,
                    StartTime = x.StartTime.Date,
                    EndTime = x.EndTime.Value.Date,
                    Status = x.Status,
                    ClientId = x.ClientId,
                    ClientName = x.Client.Name,
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.Currency.Name,
                    IsCharge = x.IsCharge,
                    ChargeType = x.ChargeType,
                    PmId = x.PMId,
                    PmName = x.PM.Name,
                });
            return await query.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.ResourceRequest_CreateNewRequestByPM, PermissionNames.ResourceRequest_CreateNewRequestForAllProject)]
        public List<GetProjectDto> GetMyProjects()
        {
            var isGetAll = this.IsGranted(PermissionNames.ResourceRequest_CreateNewRequestForAllProject);

            var queryPM = WorkScope.GetAll<Project>()
                            .Where(x => x.ProjectType != ProjectType.TRAINING)
                            .Where(x => isGetAll || x.PMId == AbpSession.UserId.Value)
                            .Where(x => x.Status != ProjectStatus.Closed)
                            .Select(x => new GetProjectDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Code = x.Code
                            });
            return queryPM.ToList();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.TrainingRequest_CreateNewRequestByPM, PermissionNames.TrainingRequest_CreateNewRequestForAllProject)]
        public List<GetProjectDto> GetMyTrainingProjects()
        {
            var isGetAll = this.IsGranted(PermissionNames.TrainingRequest_CreateNewRequestForAllProject);

            var queryPM = WorkScope.GetAll<Project>()
                            .Where(x => x.ProjectType == ProjectType.TRAINING)
                            .Where(x => isGetAll || x.PMId == AbpSession.UserId.Value)
                            .Where(x => x.Status != ProjectStatus.Closed)
                            .Select(x => new GetProjectDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Code = x.Code
                            });
            return queryPM.ToList();
        }

        [HttpGet]
        public async Task<GetProjectDto> Get(long projectId)
        {
            var query = WorkScope.GetAll<Project>().Include(x => x.Currency).Where(x => x.Id == projectId)
                                .Select(x => new GetProjectDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Code = x.Code,
                                    ProjectType = x.ProjectType,
                                    StartTime = x.StartTime.Date,
                                    EndTime = x.EndTime.Value.Date,
                                    Status = x.Status,
                                    ClientId = x.ClientId,
                                    ClientName = x.Client.Name,
                                    IsCharge = x.IsCharge,
                                    ChargeType = x.ChargeType,
                                    PmId = x.PMId,
                                    PmName = x.PM.Name,
                                    PmFullName = x.PM.FullName,
                                    PmUserName = x.PM.UserName,
                                    PmEmailAddress = x.PM.EmailAddress,
                                    PmAvatarPath = x.PM.AvatarPath,
                                    PmBranch = x.PM.BranchOld,
                                    PmUserType = x.PM.UserType,
                                    CurrencyId = x.CurrencyId,
                                    CurrencyName = x.Currency.Name,
                                    RequireTimesheetFile = x.RequireTimesheetFile,
                                    IsRequiredWeeklyReport = x.IsRequiredWeeklyReport
                                });
            return await query.FirstOrDefaultAsync();
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects)]
        public async Task<ProjectDetailDto> GetProjectDetail(long projectId)
        {
            var query = WorkScope.GetAll<ProjectTechnology>().Where(x => x.ProjectId == projectId)
                              .Select(x => new TechnologyDto
                              {
                                  Id = x.Technology.Id,
                                  Name = x.Technology.Name,
                                  Color = x.Technology.Color,
                              });
            return await WorkScope.GetAll<Project>().Where(x => x.Id == projectId)
                              .Select(x => new ProjectDetailDto
                              {
                                  ProjectId = x.Id,
                                  BriefDescription = x.BriefDescription,
                                  DetailDescription = x.DetailDescription,
                                  TechnologyUsed = x.TechnologyUsed,
                                  TechnicalProblems = x.TechnicalProblems,
                                  OtherProblems = x.OtherProblems,
                                  NewKnowledge = x.NewKnowledge,
                                  ProjectTechnologies = query.Select(s => new TechnologyDto
                                  {
                                      Id = s.Id,
                                      Name = s.Name,
                                      Color = s.Color,
                                  }).ToList()
                              }).FirstOrDefaultAsync();
        }

        [HttpPut]
        public async Task<ProjectDetailDto> UpdateProjectDetail(ProjectDetailDto input)
        {
            var project = await WorkScope.GetAsync<Project>(input.ProjectId);

            var projectTechnologies = await WorkScope.GetAll<ProjectTechnology>().Where(x => x.ProjectId == input.ProjectId).ToListAsync();
            var currentProjectTechnologiesId = projectTechnologies.Select(x => x.TechnologyId);
            var deleteTechnologyId = currentProjectTechnologiesId.Except(input.ProjectTechnologiesInput);
            var listSkillDelete = projectTechnologies.Where(x => deleteTechnologyId.Contains(x.TechnologyId));
            var listSkillInsert = input.ProjectTechnologiesInput.Where(x => !currentProjectTechnologiesId.Contains(x));

            foreach (var item in listSkillDelete)
            {
                await WorkScope.DeleteAsync<ProjectTechnology>(item);
            }

            foreach (var item in listSkillInsert)
            {
                var projectTechnology = new ProjectTechnology
                {
                    ProjectId = input.ProjectId,
                    TechnologyId = item
                };
                await WorkScope.InsertAndGetIdAsync(projectTechnology);
            }
            //Update Technologies

            await WorkScope.UpdateAsync(ObjectMapper.Map<ProjectDetailDto, Project>(input, project));
            return input;
        }

        private bool UserHasRole(long userId, string roleName)
        {
            var quser =
                    from ur in WorkScope.GetAll<UserRole, long>().Where(u => u.UserId == userId)
                    join role in WorkScope.GetAll<Role, int>().Where(s => s.Name == roleName)
                    on ur.RoleId equals role.Id into roles
                    from r in roles
                    select r.Id;
            return quser.Any();
        }

        private void AddRolePMForUser(long userId)
        {
            //Set role PM for user when user set PM of project
            var userHasRolePM = UserHasRole(userId, StaticRoleNames.Tenants.PM);
            if (!userHasRolePM)
            {
                var roleId = WorkScope.GetAll<Role, int>()
                    .Where(s => s.Name == StaticRoleNames.Tenants.PM)
                    .Select(s => s.Id)
                    .FirstOrDefault();

                WorkScope.Insert<UserRole>(new UserRole
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }
        }

        private async Task<string> getEmailAddressById(long userId)
        {
            return await WorkScope.GetAll<User>()
                .Where(x => x.Id == userId).Select(x => x.EmailAddress)
                .FirstOrDefaultAsync();
        }

        [HttpPost]
        private async Task<string> CreateProjectInTimesheetTool(ProjectDto input)
        {
            if (!(await IsEnableAutoCreateUpdateToTimsheetTool()))
            {
                Logger.Info("CreateProjectInTimesheetTool() IsEnableAutoCreateUpdateToTimsheetTool = false");
                return "update-only-project-tool";
            }
            var customerCode = await WorkScope.GetAll<Client>()
                .Where(x => x.Id == input.ClientId).Select(x => x.Code)
                .FirstOrDefaultAsync();

            var emailPM = await getEmailAddressById(input.PmId);

            return await _timesheetService.CreateProject(input.Name, input.Code, input.StartTime, input.EndTime,
                                                                       customerCode, input.ProjectType, emailPM);
        }

        [HttpPost]
        private async Task<string> ChangePmOfProjectTimesheetTool(string code, long PmId)
        {
            if (!(await IsEnableAutoCreateUpdateToTimsheetTool()))
            {
                Logger.Info("ChangePmOfProjectTimesheetTool() IsEnableAutoCreateUpdateToTimsheetTool = false");
                return "update-only-project-tool";
            }
            var emailPM = await getEmailAddressById(PmId);

            return await _timesheetService.ChangePmOfProject(code, emailPM);
        }

        [HttpPost]
        public async Task<string> Create(ProjectDto input)
        {
            var isExist = await WorkScope.GetAll<Project>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code);

            if (isExist)
                throw new UserFriendlyException("Name or Code already exist !");

            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
            {
                throw new UserFriendlyException("Start time cannot be greater than end time !");
            }

            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<Project>(input));

            var projectCheckLists = await WorkScope.GetAll<CheckListItemMandatory>()
                                .Where(x => x.ProjectType == input.ProjectType)
                                .Select(x => new ProjectCheckList
                                {
                                    ProjectId = input.Id,
                                    CheckListItemId = x.CheckListItemId,
                                    IsActive = true,
                                }).ToListAsync();

            foreach (var i in projectCheckLists)
            {
                await WorkScope.InsertAsync(i);
            }

            var pmReportActive = await WorkScope.GetAll<PMReport>().Where(x => x.IsActive).FirstOrDefaultAsync();
            if (pmReportActive == null)
                throw new UserFriendlyException("Can't find any active reports !");

            var pmReportProject = new PMReportProject
            {
                PMReportId = pmReportActive.Id,
                ProjectId = input.Id,
                Status = PMReportProjectStatus.Draft,
                ProjectHealth = ProjectHealth.Green,
                PMId = input.PmId,
                Note = null
            };
            await WorkScope.InsertAsync(pmReportProject);

            AddRolePMForUser(input.PmId);

            return await CreateProjectInTimesheetTool(input);
        }

        [HttpPost]
        public async Task<string> CloseProject(EntityDto<long> input)
        {
            var project = await WorkScope.GetAsync<Project>(input.Id);
            if (project.Status == ProjectStatus.Closed)
            {
                throw new UserFriendlyException($"Project {project.Name} is already closed");
            }
            project.Status = ProjectStatus.Closed;

            await WorkScope.UpdateAsync(ObjectMapper.Map<Project>(project));

            await _resourceManager.ReleaseAllWorkingUserFromProject(project.Id);

            var isEnableAutoCreateUpdateToTimsheetTool = await IsEnableAutoCreateUpdateToTimsheetTool();
            if (isEnableAutoCreateUpdateToTimsheetTool)
            {
                return await _timesheetService.CloseProject(project.Code);
            }
            else
            {
                Logger.Info("CloseProject() isEnableAutoCreateUpdateToTimsheetTool=" + isEnableAutoCreateUpdateToTimsheetTool);
                return "update-only-project-tool";
            }
        }

        [HttpGet]
        public async Task<List<UserOfProjectDto>> GetAllWorkingUserFromProject(long projectId)
        {
            return await _resourceManager.GetWorkingUsersOfProject(projectId);
        }

        [HttpPut]
        public async Task<string> Update(ProjectDto input)
        {
            var project = await WorkScope.GetAsync<Project>(input.Id);

            var isDuplicateNameOrCode = await WorkScope.GetAll<Project>()
                .AnyAsync(x => x.Id != input.Id && (x.Name == input.Name || x.Code == input.Code));

            if (isDuplicateNameOrCode)
                throw new UserFriendlyException("Name or Code already exist !");

            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
            {
                throw new UserFriendlyException("Start time cannot be greater than end time !");
            }

            if (input.Status == ProjectStatus.Closed && project.Status != ProjectStatus.Closed)
            {
                throw new UserFriendlyException("Please click button Close to close project");
            }

            long pmIdCurrent = project.PMId;
            string codeCurrent = project.Code;
            await WorkScope.UpdateAsync(ObjectMapper.Map<ProjectDto, Project>(input, project));

            AddRolePMForUser(input.PmId);

            if (input.PmId != pmIdCurrent && input.Code == codeCurrent)
            {
                return await ChangePmOfProjectTimesheetTool(project.Code, input.PmId);
            }
            return "update-only-project-tool";
        }

        private async Task<bool> IsEnableAutoCreateUpdateToTimsheetTool()
        {
            var dbSetting = await _settingManager.GetSettingValueForApplicationAsync(AppSettingNames.AutoUpdateProjectInfoToTimesheetTool);
            return dbSetting == "true";
        }

        public class ProjectUpdateStatus
        {
            public long Id { get; set; }

            public ProjectStatus Status { get; set; }
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_Delete,
            PermissionNames.Projects_ProductProjects_Delete,
            PermissionNames.Projects_TrainingProjects_Delete)]
        public async Task Delete(long projectID)
        {
            var project = await WorkScope.GetAsync<Project>(projectID);

            var timesheetProject = await WorkScope.GetAll<TimesheetProject>().AnyAsync(x => x.ProjectId == projectID);
            if (timesheetProject)
                throw new UserFriendlyException($"Project has Timesheet Project !");

            var pmReportProject = await WorkScope.GetAll<PMReportProject>()
                .AnyAsync(x => x.ProjectId == projectID && x.Status == PMReportProjectStatus.Sent);
            if (pmReportProject)
                throw new UserFriendlyException($"Project has Weekly Report Sent !");

            var projectUser = await WorkScope.GetAll<ProjectUser>().AnyAsync(x => x.ProjectId == projectID);
            if (projectUser)
                throw new UserFriendlyException($"Project has Project User !");

            await WorkScope.DeleteAsync(project);
        }

        [HttpPut]
        public async Task<bool> ChangeRequireWeeklyReport(long projectID)
        {
            var project = await WorkScope.GetAsync<Project>(projectID);
            project.IsRequiredWeeklyReport = !project.IsRequiredWeeklyReport;
            await WorkScope.UpdateAsync(project);
            return project.IsRequiredWeeklyReport;
        }

        #region PAGE TRAINING PROJECT

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_TrainingProjects_ViewAllProject, PermissionNames.Projects_TrainingProjects_ViewMyProjectOnly)]
        public async Task<GridResult<GetTrainingProjectDto>> GetAllTrainingProjectPaging(GridParam input)
        {
            var filterStatus = input.FilterItems != null ? input.FilterItems.FirstOrDefault(x => x.PropertyName == "status") : null;
            var filterPmId = input.FilterItems != null ? input.FilterItems.FirstOrDefault(x => x.PropertyName == "pmId" && Convert.ToInt64(x.Value) == -1) : null;
            bool hasViewRequireWRPermission = PermissionChecker.IsGranted(PermissionNames.Projects_TrainingProjects_ViewRequireWeeklyReport);
            int valueStatus = -1;
            if (filterStatus != null)
            {
                valueStatus = Convert.ToInt32(filterStatus.Value);
                input.FilterItems.Remove(filterStatus);
            }
            if (filterPmId != null)
            {
                input.FilterItems.Remove(filterPmId);
            }
            var query = from p in WorkScope.GetAll<Project>().Include(x => x.Currency)
                        .Where(x => x.ProjectType == ProjectType.TRAINING)
                        .Where(x => filterStatus != null && valueStatus > -1 ? (valueStatus == 3 ? x.Status != ProjectStatus.Closed : x.Status == (ProjectStatus)valueStatus) : true)
                        join rp in WorkScope.GetAll<PMReportProject>().Where(x => x.PMReport.IsActive) on p.Id equals rp.ProjectId into lst
                        from l in lst.DefaultIfEmpty()
                        orderby l.PM.Branch
                        select new GetTrainingProjectDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Code = p.Code,
                            StartTime = p.StartTime.Date,
                            EndTime = p.EndTime.Value.Date,
                            Status = p.Status,
                            PmId = p.PMId,
                            PmName = p.PM.Name,
                            ClientId = p.ClientId,
                            PmFullName = p.PM.FullName,
                            PmAvatarPath = p.PM.AvatarPath,
                            PmEmailAddress = p.PM.EmailAddress,
                            PmUserName = p.PM.UserName,
                            PmUserType = p.PM.UserType,
                            PmUserLevel = p.PM.UserLevel,
                            PmBranch = p.PM.BranchOld,
                            PmBranchColor = p.PM.Branch.Color,
                            PmBranchDisplayName = p.PM.Branch.DisplayName,
                            PositionId = p.PM.PositionId,
                            PositionName = p.PM.Position.ShortName,
                            PositionColor = p.PM.Position.Color,
                            IsSent = l.Status,
                            TimeSendReport = l.TimeSendReport,
                            DateSendReport = l.TimeSendReport.Value.Date,
                            Evaluation = l.Note,
                            IsRequiredWeeklyReport = hasViewRequireWRPermission ? p.IsRequiredWeeklyReport : default(bool?)
                        };
            return await query.GetGridResult(query, input);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_TrainingProjects_Create)]
        public async Task<string> CreateTrainingProject(TrainingProjectDto input)
        {
            var isExist = await WorkScope.GetAll<Project>().AnyAsync(x => x.Name == input.Name || x.Code == input.Code || x.Id == input.Id);

            if (isExist)
                throw new UserFriendlyException("Name or Code already exist !");

            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
            {
                throw new UserFriendlyException("Start time cannot be greater than end time !");
            }
            input.ProjectType = ProjectType.TRAINING;
            input.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<Project>(input));

            //var projectCheckLists = await WorkScope.GetAll<CheckListItemMandatory>()
            //                    .Where(x => x.ProjectType == ProjectType.TRAINING)
            //                    .Select(x => new ProjectCheckList
            //                    {
            //                        ProjectId = input.Id,
            //                        CheckListItemId = x.CheckListItemId,
            //                        IsActive = true,
            //                    }).ToListAsync();

            //foreach (var i in projectCheckLists)
            //{
            //    await WorkScope.InsertAsync(i);
            //}

            var pmReportActive = await WorkScope.GetAll<PMReport>()
                .OrderByDescending(s => s.Id)
                .Where(x => x.IsActive)
                .FirstOrDefaultAsync();
            if (pmReportActive == null)
                throw new UserFriendlyException("Can't find any active reports !");

            var pmReportProject = new PMReportProject
            {
                PMReportId = pmReportActive.Id,
                ProjectId = input.Id,
                Status = PMReportProjectStatus.Draft,
                ProjectHealth = ProjectHealth.Green,
                PMId = input.PmId,
                Note = null
            };
            await WorkScope.InsertAsync(pmReportProject);

            var trainingProject = new ProjectDto
            {
                Name = input.Name,
                Code = input.Code,
                ProjectType = ProjectType.TRAINING,
                PmId = input.PmId,
                StartTime = input.StartTime,
                EndTime = input.EndTime,
                ClientId = input.ClientId
            };

            AddRolePMForUser(input.PmId);

            return await CreateProjectInTimesheetTool(trainingProject);
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Projects_TrainingProjects_Edit)]
        public async Task<string> UpdateTrainingProject(TrainingProjectDto input)
        {
            input.ProjectType = ProjectType.TRAINING;
            var project = await WorkScope.GetAsync<Project>(input.Id);

            var isExist = await WorkScope.GetAll<Project>().AnyAsync(x => x.Id != input.Id && (x.Name == input.Name || x.Code == input.Code));

            if (isExist)
                throw new UserFriendlyException("Name or Code already exist !");

            if (input.EndTime.HasValue && input.StartTime.Date > input.EndTime.Value.Date)
            {
                throw new UserFriendlyException("Start time cannot be greater than end time !");
            }

            if (input.Status == ProjectStatus.Closed)
            {
                throw new UserFriendlyException("Please click button Close to close project");
            }

            ProjectStatus statusCurrent = project.Status;
            long pmIdCurrent = project.PMId;
            string codeCurrent = project.Code;

            await WorkScope.UpdateAsync(ObjectMapper.Map<TrainingProjectDto, Project>(input, project));
            AddRolePMForUser(input.PmId);

            if (input.PmId != pmIdCurrent && input.Code == codeCurrent)
            {
                return await ChangePmOfProjectTimesheetTool(project.Code, input.PmId);
            }
            return "update-only-project-tool";
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_TrainingProjects_ProjectDetail)]
        public async Task<GetTrainingProjectDto> GetDetailTrainingProject(long projectId)
        {
            var query = WorkScope.GetAll<Project>().Where(x => x.Id == projectId).Where(x => x.ProjectType == ProjectType.TRAINING)
                                .Select(x => new GetTrainingProjectDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Code = x.Code,
                                    StartTime = x.StartTime.Date,
                                    EndTime = x.EndTime.Value.Date,
                                    Status = x.Status,
                                    PmId = x.PMId,
                                    PmName = x.PM.Name,
                                    PmFullName = x.PM.FullName,
                                    PmUserName = x.PM.UserName,
                                    PmEmailAddress = x.PM.EmailAddress,
                                    PmAvatarPath = x.PM.AvatarPath,
                                    PmBranch = x.PM.BranchOld,
                                    PmUserType = x.PM.UserType,
                                    PositionId = x.PM.PositionId,
                                    PositionName = x.PM.Position.ShortName,
                                    PositionColor = x.PM.Position.Color
                                });
            return await query.FirstOrDefaultAsync();
        }

        #endregion PAGE TRAINING PROJECT

        #region PAGE PRODUCT PROJECT

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_ProductProjects_ViewAllProject, PermissionNames.Projects_ProductProjects_ViewMyProjectOnly)]
        public async Task<GridResult<ProductProjectDto>> GetAllProductProjectPaging(GridParam input)
        {
            var filterStatus = input.FilterItems != null ? input.FilterItems.FirstOrDefault(x => x.PropertyName == "status") : null;
            var filterPmId = input.FilterItems != null ? input.FilterItems.FirstOrDefault(x => x.PropertyName == "pmId" && Convert.ToInt64(x.Value) == -1) : null;
            bool hasViewRequireWRPermission = PermissionChecker.IsGranted(PermissionNames.Projects_ProductProjects_ViewRequireWeeklyReport);
            int valueStatus = -1;
            if (filterStatus != null)
            {
                valueStatus = Convert.ToInt32(filterStatus.Value);
                input.FilterItems.Remove(filterStatus);
            }
            if (filterPmId != null)
            {
                input.FilterItems.Remove(filterPmId);
            }
            var query = from p in WorkScope.GetAll<Project>()
                        .Where(x => x.ProjectType == ProjectType.PRODUCT)
                        .Where(x => filterStatus != null && valueStatus > -1 ? (valueStatus == 3 ? x.Status != ProjectStatus.Closed : x.Status == (ProjectStatus)valueStatus) : true)
                        join rp in WorkScope.GetAll<PMReportProject>().Where(x => x.PMReport.IsActive) on p.Id equals rp.ProjectId into lst
                        from l in lst.DefaultIfEmpty()
                        select new ProductProjectDto
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Code = p.Code,
                            StartTime = p.StartTime.Date,
                            EndTime = p.EndTime.Value.Date,
                            Status = p.Status,
                            PmId = p.PMId,
                            ClientId = p.ClientId,
                            PmName = p.PM.Name,
                            PmFullName = p.PM.FullName,
                            PmAvatarPath = p.PM.AvatarPath,
                            PmEmailAddress = p.PM.EmailAddress,
                            PmUserName = p.PM.UserName,
                            PmUserType = p.PM.UserType,
                            PmUserLevel = p.PM.UserLevel,
                            PositionId = p.PM.PositionId,
                            PositionName = p.PM.Position.ShortName,
                            PositionColor = p.PM.Position.Color,
                            PmBranch = p.PM.BranchOld,
                            PmBranchColor = p.PM.Branch.Color,
                            PmBranchDisplayName = p.PM.Branch.DisplayName,
                            IsSent = l.Status,
                            TimeSendReport = l.TimeSendReport,
                            DateSendReport = l.TimeSendReport.Value.Date,
                            IsRequiredWeeklyReport = hasViewRequireWRPermission ? p.IsRequiredWeeklyReport : default(bool?),
                        };
            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_ProductProjects_ProjectDetail)]
        public async Task<ProductProjectDto> GetDetailProductProject(long projectId)
        {
            var query = WorkScope.GetAll<Project>().Where(x => x.Id == projectId).Where(x => x.ProjectType == ProjectType.PRODUCT)
                                .Select(x => new ProductProjectDto
                                {
                                    Id = x.Id,
                                    Name = x.Name,
                                    Code = x.Code,
                                    StartTime = x.StartTime.Date,
                                    EndTime = x.EndTime.Value.Date,
                                    Status = x.Status,
                                    PmId = x.PMId,
                                    PmName = x.PM.Name,
                                    PmFullName = x.PM.FullName,
                                    PmUserName = x.PM.UserName,
                                    PmEmailAddress = x.PM.EmailAddress,
                                    PmAvatarPath = x.PM.AvatarPath,
                                    PmBranch = x.PM.BranchOld,
                                    PmUserType = x.PM.UserType,
                                });
            return await query.FirstOrDefaultAsync();
        }

        #endregion PAGE PRODUCT PROJECT
    }
}