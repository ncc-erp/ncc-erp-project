using Abp.Authorization;
using Abp.Configuration;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Extension;
using NccCore.Paging;
using NccCore.Uitls;
using ProjectManagement.APIs.PMReportProjectIssues;
using ProjectManagement.APIs.ProjectUsers;
using ProjectManagement.APIs.ResourceRequests.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Configuration;
using ProjectManagement.Entities;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceRequestService;
using ProjectManagement.Services.ResourceRequestService.Dto;
using ProjectManagement.Services.Talent;
using ProjectManagement.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TrainingRequests
{
    [AbpAuthorize]
    public class TrainingRequestAppService : ProjectManagementAppServiceBase
    {
        private readonly ProjectUserAppService _projectUserAppService;
        private readonly PMReportProjectIssueAppService _pMReportProjectIssueAppService;
        private readonly IUserAppService _userAppService;
        private readonly ResourceManager _resourceManager;
        private readonly ResourceRequestManager _resourceRequestManager;

        private readonly UserManager _userManager;
        private ISettingManager _settingManager;
        private KomuService _komuService;
        private readonly TalentService _talentService;

        public TrainingRequestAppService(
            ProjectUserAppService projectUserAppService,
            PMReportProjectIssueAppService pMReportProjectIssueAppService,
            KomuService komuService,
            UserManager userManager,
            ResourceManager resourceManager,
            ResourceRequestManager resourceRequestManager,
            ISettingManager settingManager,
            IUserAppService userAppService,
            TalentService talentService)
        {
            _projectUserAppService = projectUserAppService;
            _pMReportProjectIssueAppService = pMReportProjectIssueAppService;
            _komuService = komuService;
            _resourceManager = resourceManager;
            _settingManager = settingManager;
            _userAppService = userAppService;
            _userManager = userManager;
            _resourceRequestManager = resourceRequestManager;
            _talentService = talentService;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.TrainingRequest)]
        public async Task<GridResult<GetResourceRequestDto>> GetAllPaging(InputGetAllRequestResourceDto input)
        {
            var query = _resourceRequestManager.IQGetResourceRequest();

            if (!input.IsTraining)
            {
                query = query.Where(s => s.ProjectType != ProjectType.TRAINING);
            }
            else
            {
                query = query.Where(s => s.ProjectType == ProjectType.TRAINING);
            }

            if (input.SkillIds == null || input.SkillIds.IsEmpty())
            {
                return await query.GetGridResult(query, input);
            }
            if (input.SkillIds.Count() == 1 || !input.IsAndCondition)
            {
                var qRequestIdsHaveAnySkill = QueryResourceRequestIdsHaveAnySkill(input.SkillIds).Distinct();
                query = from request in query
                        join requestId in qRequestIdsHaveAnySkill on request.Id equals requestId
                        select request;

                return await query.GetGridResult(query, input);
            }

            var requestIds = await QetResourceRequestIdsHaveAllSkill(input.SkillIds);
            query = query.Where(s => requestIds.Contains(s.Id));

            return await query.GetGridResult(query, input);
        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<GetResourceRequestDto>> GetAllByProject(long projectId, ResourceRequestStatus? status)
        {
            var query = _resourceRequestManager.IQGetResourceRequest()
                .Where(x => x.ProjectId == projectId)
                .Where(s => !status.HasValue || s.Status == status)
                .OrderByDescending(x => x.Priority)
                .ThenBy(x => x.TimeNeed);
            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<List<GetResourceRequestDto>> Create(CreateResourceRequestDto input)
        {

            if (input.Quantity <= 0)
                throw new UserFriendlyException("Quantity must be >= 1");

            if (!input.SkillIds.Any())
                throw new UserFriendlyException("Select at least 1 skill");

            List<long> createdRequestIds = new List<long>();
            var request = ObjectMapper.Map<ResourceRequest>(input);
            request.Level = UserLevel.Intern_0;
            request.Id = await WorkScope.InsertAndGetIdAsync(request);
            createdRequestIds.Add(request.Id);
            CurrentUnitOfWork.SaveChanges();
            foreach (var skillId in input.SkillIds)
            {
                var requestSkill = new ResourceRequestSkill()
                {
                    ResourceRequestId = request.Id,
                    SkillId = skillId,
                    Quantity = 1
                };

                await WorkScope.InsertAsync(requestSkill);
            }

            //SendKomuNotify(model.Name, project.Name, model.Status);

            CurrentUnitOfWork.SaveChanges();

            var listRequestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => createdRequestIds.Contains(s.Id))
                .ToListAsync();

            await notifyToKomu(listRequestDto.FirstOrDefault(), Action.Create, input.Quantity);

            return listRequestDto;
        }

        [HttpGet]
        public async Task<GetResourceRequestDto> GetById(long requestId)
        {
            return await _resourceRequestManager.IQGetResourceRequest()
                .Where(q => q.Id == requestId)
                .FirstOrDefaultAsync();
        }

        [HttpPut]
        [AbpAuthorize]
        public async Task<GetResourceRequestDto> Update(UpdateResourceRequestDto input)
        {
            if (input.SkillIds == null || input.SkillIds.IsEmpty())
            {
                throw new UserFriendlyException("Skill can't be null or empty");
            }

            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(input.Id);
            ObjectMapper.Map(input, resourceRequest);
            await WorkScope.UpdateAsync(resourceRequest);

            var dbRequestSkills = await WorkScope.GetAll<ResourceRequestSkill>()
                                                .Where(p => p.ResourceRequestId == input.Id)
                                                .Select(p => new { p.Id, p.SkillId })
                                                .ToListAsync();

            var dbSkillIds = dbRequestSkills.Select(s => s.SkillId).ToList();

            var skillIdsToAdd = input.SkillIds.Except(dbSkillIds);
            var requestSkillIdsToRemove = dbRequestSkills.Where(s => !input.SkillIds.Contains(s.SkillId))
                .Select(s => s.Id);

            foreach (var skillId in skillIdsToAdd)
            {
                var skillModel = new ResourceRequestSkill()
                {
                    ResourceRequestId = input.Id,
                    SkillId = skillId,
                    Quantity = 1
                };
                await WorkScope.InsertAsync(skillModel);
            }

            foreach (var requestSkillId in requestSkillIdsToRemove)
            {
                await WorkScope.DeleteAsync<ResourceRequestSkill>(requestSkillId);
            }
            await CurrentUnitOfWork.SaveChangesAsync();
            return await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == input.Id)
                .FirstOrDefaultAsync();
        }

        [HttpPut]
        public async Task<GetResourceRequestDto> UpdateMyRequest(UpdateResourceRequestDto input)
        {
            await CheckRequestIsForMyProject(input.Id, input.ProjectId);
            return await Update(input);
        }

        [HttpDelete]
        [AbpAuthorize]
        public async Task Delete(long resourceRequestId)
        {
            var IsPlannedResource = await WorkScope.GetAll<ProjectUser>()
                .Where(s => s.ResourceRequestId == resourceRequestId)
                .AnyAsync();
            if (IsPlannedResource)
            {
                throw new UserFriendlyException($"Request Id {resourceRequestId} already planned resource.");
            }

            await WorkScope.DeleteAsync<ResourceRequest>(resourceRequestId);
        }

        [HttpDelete]
        public async Task DeleteMyRequest(long resourceRequestId)
        {
            await CheckRequestIsForMyProject(resourceRequestId, null);
            await Delete(resourceRequestId);
        }

        private async Task CheckRequestIsForMyProject(long requestId, long? newProjectId)
        {
            var project = await WorkScope.GetAll<ResourceRequest>()
                .Where(s => s.Id == requestId)
                .Select(s => new
                {
                    s.Project.PMId,
                    s.ProjectId
                })
                .FirstOrDefaultAsync();
            if (project == default)
            {
                throw new UserFriendlyException($"Request Id {requestId} is not exist");
            }

            if (newProjectId.HasValue && project.ProjectId != newProjectId)
            {
                throw new UserFriendlyException($"Request Id {requestId} is for your project. You can't change to other project");
            }

            var isGrantedCancelAll = IsGranted(PermissionNames.TrainingRequest_CancelAllRequest);

            if (!isGrantedCancelAll && project.PMId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException($"Request Id {requestId} is for project that you are NOT PM.");
            }
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.TrainingRequest_CancelAllRequest, PermissionNames.TrainingRequest_CancelMyRequest)]
        public async Task<GetResourceRequestDto> CancelRequest(long requestId)
        {
            await CheckRequestIsForMyProject(requestId, null);
            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(requestId);

            resourceRequest.Status = ResourceRequestStatus.CANCELLED;

            await WorkScope.UpdateAsync(resourceRequest);

            var requestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == requestId)
                .FirstOrDefaultAsync();

            if (resourceRequest.IsRecruitmentSend)
                await _talentService.CancelRequest(new Services.Talent.Dtos.CloseResourceRequestDto
                {
                    ResourceRequestId = requestId
                });

            await notifyToKomu(requestDto, Action.Cancel, null);
            return requestDto;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<UpdateRequestNoteDto> UpdatePMNote(UpdateRequestNoteDto input)
        {
            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(input.ResourceRequestId);

            resourceRequest.PMNote = input.Note;

            await WorkScope.UpdateAsync(resourceRequest);

            return input;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<UpdateRequestNoteDto> UpdateHPMNote(UpdateRequestNoteDto input)
        {
            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(input.ResourceRequestId);

            resourceRequest.DMNote = input.Note;

            await WorkScope.UpdateAsync(resourceRequest);

            return input;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<ResourceRequestSetDoneDto> SetDone(ResourceRequestSetDoneDto input)
        {
            var request = await WorkScope.GetAll<ResourceRequest>()
                .Where(s => s.Id == input.RequestId)
                .Select(s => new
                {
                    Request = s,
                    PlanUserInfo = s.ProjectUsers.OrderByDescending(x => x.CreationTime).FirstOrDefault()
                }).FirstOrDefaultAsync();

            if (request == default)
            {
                throw new UserFriendlyException("Not found Request with Id " + input.RequestId);
            }

            if (request.PlanUserInfo == null)
            {
                throw new UserFriendlyException("You have to plan resource for this request first");
            }

            await _resourceManager.ConfirmJoinProject(request.PlanUserInfo.Id, input.StartTime, true);

            request.Request.Status = ResourceRequestStatus.DONE;
            request.Request.TimeDone = DateTimeUtils.GetNow();
            await WorkScope.UpdateAsync(request.Request);
            return input;
        }

        private async Task notifyToKomu(GetResourceRequestDto requestDto, Action action, int? quantity)
        {
            var sessionUser = await _resourceManager.getSessionKomuUserInfo();
            StringBuilder sbKomuMessage = new StringBuilder();
            sbKomuMessage.AppendLine($"{sessionUser.KomuAccountInfo} {ActionName(action)}");
            sbKomuMessage.AppendLine($"```{requestDto.KomuInfo()}");
            if (action == Action.Create)
            {
                sbKomuMessage.AppendLine($"Quantity: {quantity.Value}");
            }
            sbKomuMessage.AppendLine($"```");

            if (action == Action.Plan && requestDto.PlanUserInfo != null)
            {
                sbKomuMessage.AppendLine($"Planned Info:");
                sbKomuMessage.Append($"```{requestDto.PlanUserInfo.KomuInfo()}```");
            }

            SendKomu(sbKomuMessage);
        }

        private string ActionName(Action action)
        {
            switch (action)
            {
                case Action.Create:
                    return "has **created** a training request:";

                case Action.Cancel:
                    return "has **cancelled** the training request:";

                case Action.Plan:
                    return "has **planned** for the training request:";

                case Action.Done:
                    return "has **done** the training request:";
            }
            return "";
        }

        private void SendKomu(StringBuilder komuMessage)
        {
            var trainingRequestChannel = SettingManager.GetSettingValue(AppSettingNames.TrainingRequestChannel);
            if (!string.IsNullOrEmpty(trainingRequestChannel))
            {
                _komuService.NotifyToChannelId(new KomuMessage
                {
                    CreateDate = DateTimeUtils.GetNow(),
                    Message = komuMessage.ToString()
                },
               trainingRequestChannel);
            }
        }

        [HttpGet]
        public async Task<ResourceRequestPlanDto> GetResourceRequestPlan(long projectUserId)
        {
            var projectUser = await WorkScope.GetAsync<ProjectUser>(projectUserId);

            if (projectUser == null)
                return null;
            else
                return new ResourceRequestPlanDto()
                {
                    ProjectUserId = projectUser.Id,
                    UserId = projectUser.UserId,
                    StartTime = projectUser.StartTime,
                    ProjectRole = projectUser.ProjectRole,
                    ResourceRequestId = projectUser.ResourceRequestId
                };
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PlanUserInfoDto> CreateResourceRequestPlan(ResourceRequestPlanDto input)
        {
            if (!input.ResourceRequestId.HasValue)
            {
                throw new UserFriendlyException("ResourceRequestId can't be null");
            }

            var request = await WorkScope.GetAll<ResourceRequest>()
                .Where(s => s.Id == input.ResourceRequestId.Value)
                .Select(s => new { s.ProjectId })
                .FirstOrDefaultAsync();

            if (request == default)
                throw new UserFriendlyException("Not found resource request Id " + input.ResourceRequestId);

            var activeReportId = await _resourceManager.GetActiveReportId();

            var projectUser = new ProjectUser()
            {
                UserId = input.UserId,
                ProjectId = request.ProjectId,
                ProjectRole = input.ProjectRole,
                AllocatePercentage = 100,
                StartTime = input.StartTime,
                Status = ProjectUserStatus.Future,
                ResourceRequestId = input.ResourceRequestId,
                PMReportId = activeReportId,
                IsPool = false,
                Note = "Planned for resource request Id " + input.ResourceRequestId
            };

            projectUser.Id = await WorkScope.InsertAndGetIdAsync(projectUser);
            CurrentUnitOfWork.SaveChanges();

            var requestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == input.ResourceRequestId)
                .FirstOrDefaultAsync();

            await notifyToKomu(requestDto, Action.Plan, null);

            return requestDto.PlanUserInfo;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PlanUserInfoDto> UpdateResourceRequestPlan(ResourceRequestPlanDto input)
        {
            var projectUser = WorkScope.Get<ProjectUser>(input.ProjectUserId);

            if (projectUser == null)
                throw new UserFriendlyException($"Not found ProjectUser with id : {input.ProjectUserId}");

            projectUser.UserId = input.UserId;
            projectUser.StartTime = input.StartTime;
            projectUser.ProjectRole = input.ProjectRole;

            await WorkScope.UpdateAsync(projectUser);
            CurrentUnitOfWork.SaveChanges();

            return (await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == input.ResourceRequestId)
                .FirstOrDefaultAsync()).PlanUserInfo;
        }

        [HttpDelete]
        [AbpAuthorize]
        public async Task DeleteResourceRequestPlan(long requestId)
        {
            var request = await WorkScope.GetAll<ResourceRequest>()
                .Where(s => s.Id == requestId)
                .Select(s => new
                {
                    s.Status,
                    PUs = s.ProjectUsers.Where(s => s.Status == ProjectUserStatus.Future && s.AllocatePercentage > 0).ToList()
                }).FirstOrDefaultAsync();

            if (request == default)
            {
                throw new UserFriendlyException("Not found request with Id " + requestId);
            }

            if (request.Status == ResourceRequestStatus.DONE)
            {
                throw new UserFriendlyException("Request already DONE. You can't delete Planned Resource");
            }

            foreach (var pu in request.PUs)
            {
                pu.IsDeleted = true;
            }

            CurrentUnitOfWork.SaveChanges();
        }

        private IQueryable<long> QueryResourceRequestIdsHaveAnySkill(List<long> skillIds)
        {
            if (skillIds == null || skillIds.IsEmpty())
            {
                throw new Exception("skillIds null or empty");
            }
            return WorkScope.GetAll<ResourceRequestSkill>()
                   .Where(s => skillIds.Contains(s.SkillId))
                   .Select(s => s.ResourceRequestId);
        }

        private async Task<List<long>> QetResourceRequestIdsHaveAllSkill(List<long> skillIds)
        {
            if (skillIds == null || skillIds.IsEmpty())
            {
                throw new Exception("skillIds null or empty");
            }

            var result = await WorkScope.GetAll<ResourceRequestSkill>()
                    .Where(s => skillIds[0] == s.SkillId)
                    .Select(s => s.ResourceRequestId)
                    .Distinct()
                    .ToListAsync();

            if (result == null || result.IsEmpty())
            {
                return new List<long>();
            }

            for (var i = 1; i < skillIds.Count(); i++)
            {
                var userIds = await WorkScope.GetAll<ResourceRequestSkill>()
                    .Where(s => skillIds[i] == s.SkillId)
                    .Select(s => s.ResourceRequestId)
                    .Distinct()
                    .ToListAsync();

                result = result.Intersect(userIds).ToList();

                if (result == null || result.IsEmpty())
                {
                    return new List<long>();
                }
            }

            return result;
        }

        [HttpGet]
        public List<IDNameDto> GetRequestLevels()
        {
            var result = new List<IDNameDto>();
            result.Add(new IDNameDto { Id = UserLevel.AnyLevel.GetHashCode(), Name = "Any Level" });
            result.Add(new IDNameDto { Id = UserLevel.Intern_3.GetHashCode(), Name = "Intern" });
            result.Add(new IDNameDto { Id = UserLevel.Fresher.GetHashCode(), Name = "Fresher" });
            result.Add(new IDNameDto { Id = UserLevel.Junior.GetHashCode(), Name = "Junior" });
            result.Add(new IDNameDto { Id = UserLevel.Middle.GetHashCode(), Name = "Middle" });
            result.Add(new IDNameDto { Id = UserLevel.Senior.GetHashCode(), Name = "Senior" });
            return result;
        }

        [HttpGet]
        public List<IDNameDto> GetPriorities()
        {
            return Enum.GetValues(typeof(Priority))
                             .Cast<Priority>()
                             .Select(p => new IDNameDto()
                             {
                                 Id = p.GetHashCode(),
                                 Name = p.ToString()
                             })
                             .ToList();
        }

        [HttpGet]
        public List<IDNameDto> GetStatuses()
        {
            return Enum.GetValues(typeof(ResourceRequestStatus))
                             .Cast<ResourceRequestStatus>()
                             .Select(p => new IDNameDto()
                             {
                                 Id = p.GetHashCode(),
                                 Name = p.ToString()
                             })
                             .ToList();
        }

        [HttpGet]
        public List<IDNameDto> GetProjectUserRoles()
        {
            return Enum.GetValues(typeof(ProjectUserRole))
                .Cast<ProjectUserRole>()
                .Select(q => new IDNameDto
                {
                    Id = q.GetHashCode(),
                    Name = q.ToString()
                }).ToList();
        }

        [HttpGet]
        public List<IDNameDto> GetTrainingRequestLevels()
        {
            var result = new List<IDNameDto>();
            result.Add(new IDNameDto { Id = UserLevel.Intern_3.GetHashCode(), Name = "Intern" });
            return result;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<List<GetResourceRequestDto>> CreateTraining(CreateResourceRequestDto input)
        {
            if (input.Quantity <= 0)
            {
                throw new UserFriendlyException("Quantity must be >= 1");
            }

            if (!input.SkillIds.Any())
            {
                throw new UserFriendlyException("Select at least 1 skill");
            }

            List<long> createdRequestIds = new List<long>();

            var request = ObjectMapper.Map<ResourceRequest>(input);
            request.Id = await WorkScope.InsertAndGetIdAsync(request);
            createdRequestIds.Add(request.Id);
            CurrentUnitOfWork.SaveChanges();
            foreach (var skillId in input.SkillIds)
            {
                var requestSkill = new ResourceRequestSkill()
                {
                    ResourceRequestId = request.Id,
                    SkillId = skillId,
                    Quantity = 1
                };

                await WorkScope.InsertAsync(requestSkill);
            }
            CurrentUnitOfWork.SaveChanges();

            var listRequestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => createdRequestIds.Contains(s.Id))
                .ToListAsync();

            return listRequestDto;
        }

        private enum Action : byte
        {
            Create = 1,
            Cancel = 2,
            Plan = 3,
            Done = 4
        }
    }
}