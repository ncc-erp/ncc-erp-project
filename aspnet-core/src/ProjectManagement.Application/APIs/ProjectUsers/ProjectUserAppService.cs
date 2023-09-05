using Abp.Authorization;
using Abp.Configuration;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Uitls;
using ProjectManagement.APIs.ProjectUsers.Dto;
using ProjectManagement.APIs.ResourceRequests.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Constants;
using ProjectManagement.Entities;
using ProjectManagement.NccCore.Helper;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceManager.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUsers
{
    [AbpAuthorize]
    public class ProjectUserAppService : ProjectManagementAppServiceBase, IProjectUserAppService
    {
        private readonly ResourceManager _resourceManager;
        private ISettingManager _settingManager;
        private KomuService _komuService;

        public ProjectUserAppService(
            KomuService komuService,
            ResourceManager resourceManager,
            ISettingManager settingManager) : base()
        {
            _resourceManager = resourceManager;
            _komuService = komuService;
            _settingManager = settingManager;
        }

        [HttpGet]
        public async Task<List<GetProjectUserDto>> GetAllByProject(long projectId, bool viewHistory)
        {
            var query = WorkScope.GetAll<ProjectUser>().Where(x => x.ProjectId == projectId && x.IsFutureActive)
                        .Where(x => viewHistory || x.Status != ProjectUserStatus.Past && (x.Status == ProjectUserStatus.Present ? x.AllocatePercentage > 0 : true))
                        .Where(x => x.User.UserType != UserType.FakeUser)
                        .OrderByDescending(x => x.CreationTime)
                        .Select(x => new GetProjectUserDto
                        {
                            Id = x.Id,
                            UserId = x.UserId,
                            FullName = x.User.FullName,
                            ProjectId = x.ProjectId,
                            ProjectName = x.Project.Name,
                            ProjectRole = x.ProjectRole.ToString(),
                            AllocatePercentage = x.AllocatePercentage,
                            StartTime = x.StartTime,
                            Status = x.Status.ToString(),
                            IsExpense = x.IsExpense,
                            ResourceRequestId = x.ResourceRequestId,
                            PMReportId = x.PMReportId,
                            IsFutureActive = x.IsFutureActive,
                            AvatarPath = x.User.AvatarPath,
                            Branch = x.User.BranchOld,
                            EmailAddress = x.User.EmailAddress,
                            UserName = x.User.UserName,
                            UserType = x.User.UserType,
                            Note = x.Note
                        });
            return await query.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize()]
        public async Task<List<UserOfProjectDto>> GetAllWorkingUserByProject(long projectId, bool viewHistory)
        {
            var query = _resourceManager.QueryUsersOfProject(projectId);
            if (!viewHistory)
            {
                query = query.Where(x => x.PUStatus == ProjectUserStatus.Present && x.AllocatePercentage > 0);
            }
            else
            {
                query = query.Where(x => x.PUStatus != ProjectUserStatus.Future);
            }

            return await query.ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize()]
        public async Task<List<UserOfProjectDto>> GetAllPlannedUserByProject(long projectId)
        {
            var activeReportId = await _resourceManager.GetActiveReportId();
            var query = _resourceManager.QueryPlansOfProject(projectId);
            return await query.Where(x => x.PMReportId == activeReportId).ToListAsync();
        }

        [HttpGet]
        [AbpAuthorize()]
        public async Task<List<ProjectOfUserDto>> GetAllWorkingProjectByUserId(long userId)
        {
            var query = _resourceManager.QueryWorkingProjectsOfUser(userId);

            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool)]
        public async Task AddUserToOutSourcingProject(AddResourceToProjectDto input)
        {
            var allowMoveEmployeeToOtherProject
                = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject);
            await _resourceManager.CreatePresentProjectUserAndNofity(input, allowMoveEmployeeToOtherProject);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool)]
        public async Task AddUserToProductProject(AddResourceToProjectDto input)
        {
            var allowMoveEmployeeToOtherProject
                = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject);
            await _resourceManager.CreatePresentProjectUserAndNofity(input, allowMoveEmployeeToOtherProject);
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromPool)]
        public async Task AddUserToTrainingProject(AddResourceToProjectDto input)
        {
            var allowMoveEmployeeToOtherProject
                = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_CurrentResource_AddNewResourceFromOtherProject);
            await _resourceManager.CreatePresentProjectUserAndNofity(input, allowMoveEmployeeToOtherProject);
        }

        [HttpPut]
        [AbpAuthorize()]
        public async Task<IActionResult> UpdateCurrentResourceDetail(UpdateProjectUserDto input)
        {
            var projectUser = await WorkScope.GetAsync<ProjectUser>(input.Id);
            //var pmReportActive = await WorkScope.GetAll<PMReport>().Where(x => x.IsActive).FirstOrDefaultAsync();
            //if (pmReportActive == null)
            //    throw new UserFriendlyException("Can't find any active reports !");

            projectUser.ProjectRole = input.ProjectRole;
            projectUser.IsPool = input.IsPool;
            projectUser.StartTime = input.StartTime;
            //model.PMReportId = pmReportActive.Id;
            await WorkScope.UpdateAsync(projectUser);

            return new OkObjectResult("Update succesful");
        }

        [HttpPost]
        [AbpAuthorize()]
        public async Task ReleaseUser(ReleaseUserToPoolDto input)
        {
            await _resourceManager.ReleaseWorkingUserFromProject(input);
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

        //[HttpGet]
        //[AbpAuthorize()]
        //public async Task ConfirmJoinProject(long projectUserId, DateTime startTime)
        //{
        //    var allowConfirmMoveEmployeeToOtherProject
        //    = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_);
        //    await _resourceManager.ConfirmJoinProject(projectUserId, startTime, allowConfirmMoveEmployeeToOtherProject);
        //}

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther,
            PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject)]
        public async Task ConfirmJoinProjectOutsourcing(long projectUserId, DateTime startTime)
        {
            var allowConfirmMoveEmployeeToOtherProject
            = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_OutsourcingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther);
            await _resourceManager.ConfirmJoinProject(projectUserId, startTime, allowConfirmMoveEmployeeToOtherProject);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther,
            PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject)]
        public async Task ConfirmJoinProjectProduct(long projectUserId, DateTime startTime)
        {
            var allowConfirmMoveEmployeeToOtherProject
            = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_ProductProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther);
            await _resourceManager.ConfirmJoinProject(projectUserId, startTime, allowConfirmMoveEmployeeToOtherProject);
        }

        [HttpGet]
        [AbpAuthorize(PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther,
            PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmPickEmployeeFromPoolToProject)]
        public async Task ConfirmJoinProjectTraining(long projectUserId, DateTime startTime)
        {
            var allowConfirmMoveEmployeeToOtherProject
            = await PermissionChecker.IsGrantedAsync(PermissionNames.Projects_TrainingProjects_ProjectDetail_TabResourceManagement_PlannedResource_ConfirmMoveEmployeeWorkingOnAProjectToOther);
            await _resourceManager.ConfirmJoinProject(projectUserId, startTime, allowConfirmMoveEmployeeToOtherProject);
        }

        [HttpPost]
        [AbpAuthorize()]
        public async Task EditProjectUserPlan(EditProjectUserDto input)
        {
            await _resourceManager.EditProjectUserPlan(input);
        }

        [HttpPost]
        [AbpAuthorize()]
        public async Task PlanNewResourceToProject(InputPlanResourceDto input)
        {
            await _resourceManager.AddFuturePU(input);
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAllProjectUserInProject(long projectId)
        {
            var projectUser = WorkScope.GetAll<ProjectUser>().Where(x => x.ProjectId == projectId && x.Status != ProjectUserStatus.Past).Select(x => x.UserId);

            var query = WorkScope.GetAll<User>().Where(x => x.IsActive && projectUser.Contains(x.Id))
                        .Select(x => new UserDto
                        {
                            Id = x.Id,
                            FullName = x.FullName,
                            AvatarPath = x.AvatarPath,
                            UserType = x.UserType,
                            UserLevel = x.UserLevel,
                            Branch = x.BranchOld,
                            EmailAddress = x.EmailAddress,
                            UserName = x.UserName,
                        });
            return await query.ToListAsync();
        }

        [HttpGet]
        public async Task<GetProjectUserDto> Get(long projectUserId)
        {
            var query = WorkScope.GetAll<ProjectUser>().Where(x => x.Id == projectUserId)
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
                                    Note = x.Note
                                });
            return await query.FirstOrDefaultAsync();
        }

        [HttpGet]
        public async Task<List<GetProjectUserDto>> GetProjectHistoryByUser(long UserId)
        {
            var query = WorkScope
                .GetAll<ProjectUser>()
                .Where(x => x.UserId == UserId & x.Status != ProjectUserStatus.Future)
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
                                    Note = x.Note,
                                    PMName = x.Project.PM.FullName
                                }).OrderByDescending(x => x.StartTime);
            return await query.ToListAsync();
        }

        [HttpPost]
        public async Task<ProjectUserDto> Create(ProjectUserDto model)
        {
            var isExistProjectUser = await WorkScope.GetAll<ProjectUser>().AnyAsync(x => x.ProjectId == model.ProjectId && x.UserId == model.UserId
                                    && x.Status == model.Status && x.StartTime.Date == model.StartTime.Date && x.ProjectRole == x.ProjectRole
                                    && x.AllocatePercentage == model.AllocatePercentage);
            if (isExistProjectUser)
                throw new UserFriendlyException("User already exist in project !");
            if (model.Status == ProjectUserStatus.Past)
                throw new UserFriendlyException("Can't add people to the past !");
            var pmReportActive = await WorkScope.GetAll<PMReport>().Where(x => x.IsActive).FirstOrDefaultAsync();
            if (pmReportActive == null)
                throw new UserFriendlyException("Can't find any active reports !");

            if (model.AllocatePercentage > 0)
            {
                var isInactiveUser = await WorkScope.GetAll<User>().AnyAsync(x => x.Id == model.UserId && !x.IsActive);
                if (isInactiveUser)
                    throw new UserFriendlyException("Can't add people is inactive !");
            }

            model.IsFutureActive = true;
            model.PMReportId = pmReportActive.Id;
            model.Status = model.StartTime.Date > DateTime.Now.Date ? ProjectUserStatus.Future : ProjectUserStatus.Present;
            model.Id = await WorkScope.InsertAndGetIdAsync(ObjectMapper.Map<ProjectUser>(model));

            if (model.Status == ProjectUserStatus.Present)
            {
                var projectUsers = await WorkScope.GetAll<ProjectUser>().Where(x => x.Id != model.Id && x.ProjectId == model.ProjectId && x.UserId == model.UserId && x.Status == ProjectUserStatus.Present).ToListAsync();
                foreach (var item in projectUsers)
                {
                    item.Status = ProjectUserStatus.Past;
                    await WorkScope.UpdateAsync(item);
                }
            }
            var project = await WorkScope.GetAll<Project>().FirstOrDefaultAsync(x => x.Id == model.ProjectId);
            if (project == null)
                throw new UserFriendlyException("Project doesn't exist");
            var pm = await WorkScope.GetAsync<User>(AbpSession.UserId.Value);
            var user = await WorkScope.GetAsync<User>(model.UserId);
            var pmUserName = UserHelper.GetUserName(pm.EmailAddress);
            var userName = UserHelper.GetUserName(user.EmailAddress);
            if (pm != null && !pm.KomuUserId.HasValue)
            {
                pm.KomuUserId = await _komuService.GetKomuUserId(new KomuUserDto { Username = pmUserName ?? pm.UserName });
                await WorkScope.UpdateAsync<User>(pm);
            }
            var message = new StringBuilder();
            if (model.AllocatePercentage == 0)
                message.AppendLine($"Từ ngày **{model.StartTime:dd/MM/yyyy}**, PM {(pm.KomuUserId.HasValue ? "<@" + pm.KomuUserId + ">" : "**" + (pmUserName ?? pm.UserName) + "**")} release **{userName ?? user.UserName}** ra khỏi dự án **{project.Name}**.");
            else
                message.AppendLine($"Từ ngày **{model.StartTime:dd/MM/yyyy}**, PM {(pm.KomuUserId.HasValue ? "<@" + pm.KomuUserId + ">" : "**" + (pmUserName ?? pm.UserName) + "**")} request **{userName ?? user.UserName}** làm việc ở dự án **{project.Name}**.");
            _komuService.NotifyToChannel(new KomuMessage
            {
                UserName = pmUserName ?? pm.UserName,
                Message = message.ToString(),
                CreateDate = DateTimeUtils.GetNow(),
            }, ChannelTypeConstant.PM_CHANNEL);
            return model;
        }

        [HttpPut]
        public async Task<ProjectUserDto> Update(ProjectUserDto model)
        {
            var projectUser = await WorkScope.GetAsync<ProjectUser>(model.Id);

            if (projectUser.Status == ProjectUserStatus.Past)
                throw new UserFriendlyException("Can't edit people in the past !");

            if (model.Status == ProjectUserStatus.Past)
                throw new UserFriendlyException("Can't edit people to the past !");

            if (model.ResourceRequestId != null && model.StartTime.Date < DateTime.Now.Date)
            {
                throw new UserFriendlyException("Can't add user at past time !");
            }

            if (projectUser.Status == ProjectUserStatus.Future && model.Status == ProjectUserStatus.Present)
            {
                var projectUsers = await WorkScope.GetAll<ProjectUser>().Where(x => x.Id != model.Id && x.ProjectId == model.ProjectId && x.UserId == model.UserId && x.Status == ProjectUserStatus.Present).ToListAsync();
                foreach (var item in projectUsers)
                {
                    item.Status = ProjectUserStatus.Past;
                    await WorkScope.UpdateAsync(item);
                }
            }

            var pmReportActive = await WorkScope.GetAll<PMReport>().Where(x => x.IsActive).FirstOrDefaultAsync();
            if (pmReportActive == null)
                throw new UserFriendlyException("Can't find any active reports !");

            model.IsFutureActive = true;
            model.PMReportId = pmReportActive.Id;
            await WorkScope.UpdateAsync(ObjectMapper.Map<ProjectUserDto, ProjectUser>(model, projectUser));

            return model;
        }

        [HttpDelete]
        public async Task Delete(long projectUserId)
        {
            var projectUser = await WorkScope.GetAsync<ProjectUser>(projectUserId);

            await WorkScope.DeleteAsync(projectUser);
        }

        [HttpGet]
        public List<IDNameDto> GetProjectUserRoles()
        {
            return Enum.GetValues(typeof(ProjectUserRole))
                             .Cast<ProjectUserRole>()
                             .Select(p => new IDNameDto()
                             {
                                 Id = p.GetHashCode(),
                                 Name = p.ToString()
                             })
                             .ToList();
        }

        [HttpPut]
        public async Task EditCurentResourceNote(long id, string note)
        {
            var item = WorkScope.Get<ProjectUser>(id);
            item.Note = note;
            await WorkScope.UpdateAsync(item);
        }
    }
}