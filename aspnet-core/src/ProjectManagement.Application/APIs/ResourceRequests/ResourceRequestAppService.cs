﻿using Abp.Authorization;
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
using ProjectManagement.Constants;
using ProjectManagement.Entities;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceRequestService;
using ProjectManagement.Services.ResourceRequestService.Dto;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Services.Talent;
using ProjectManagement.UploadFilesService;
using ProjectManagement.Users;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests
{
    [AbpAuthorize]
    public class ResourceRequestAppService : ProjectManagementAppServiceBase
    {
        private readonly ProjectUserAppService _projectUserAppService;
        private readonly PMReportProjectIssueAppService _pMReportProjectIssueAppService;
        private readonly IUserAppService _userAppService;
        private readonly ResourceManager _resourceManager;
        private readonly ResourceRequestManager _resourceRequestManager;
        private readonly UploadFileService _uploadFileService;

        private readonly UserManager _userManager;
        private ISettingManager _settingManager;
        private KomuService _komuService;
        private readonly TalentService _talentService;

        public ResourceRequestAppService(
            ProjectUserAppService projectUserAppService,
            PMReportProjectIssueAppService pMReportProjectIssueAppService,
            KomuService komuService,
            UserManager userManager,
            ResourceManager resourceManager,
            ResourceRequestManager resourceRequestManager,
            ISettingManager settingManager,
            IUserAppService userAppService,
            TalentService talentService,
            UploadFileService uploadFileService)
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
            _uploadFileService = uploadFileService;
        }


        [HttpPost]
        public async Task<ResourceRequestCV> AddCV(ResourceRequestCVDto input)
        {
            var isExist = await WorkScope.GetAll<ResourceRequestCV>()
                                       .AnyAsync(s => s.CVName == input.CVName && s.ResourceRequestId == input.ResourceRequestId);
            if (isExist && !input.CVName.IsEmpty())
            {
                throw new UserFriendlyException(string.Format("CV Name {0} is already exist !", input.CVName));
            }
            var entity = ObjectMapper.Map<ResourceRequestCV>(input);
            await WorkScope.InsertAsync<ResourceRequestCV>(entity);
            return entity;
        }

        [HttpGet]
        public async Task<List<ResourceRequestCVDto>> GetResourceRequestCV(long resourceRequestId)
        {
            return await WorkScope.GetAll<Entities.ResourceRequestCV>()
                    .Include(s => s.CvStatus)
                    .Where(rs => rs.ResourceRequestId == resourceRequestId)
                   .Select(s => new ResourceRequestCVDto
                   {
                       UserId = s.UserId,
                       User = new UserBaseDto
                       {
                           EmailAddress = s.User.EmailAddress,
                           AvatarPath = s.User.AvatarPath,
                           UserType = s.User.UserType,
                           BranchColor = s.User.Branch.Color,
                           BranchDisplayName = s.User.Branch.DisplayName,
                           PositionColor = s.User.Position.Color,
                           PositionName = s.User.Position.Name,
                           PositionId = s.User.Position.Id,
                           UserLevel = s.User.UserLevel,
                           FullName = s.User.FullName,
                           Branch = s.User.BranchOld,
                           Id = s.User.Id,
                       },
                       ResourceRequestId = s.ResourceRequestId,
                       Status = s.Status,
                       CVName = s.CVName,
                       CVPath = s.CVPath,
                       Note = s.Note,
                       KpiPoint = s.KpiPoint,
                       InterviewDate = s.InterviewDate,
                       SendCVDate = s.SendCVDate,
                       Id = s.Id,
                       CvStatusId = s.CvStatusId,
                       CvStatus = s.CvStatus,
                   })
                   .ToListAsync();
        }
        [HttpGet]
        public async Task<ResourceRequestCV> GetResourceRequestCVById(long resourceRequestCVId)
        {
            var check = await WorkScope.GetAll<ResourceRequestCV>().AnyAsync(s => s.Id == resourceRequestCVId);
            if (!check)
            {
                throw new UserFriendlyException(string.Format("CV with Id {0} is not exits !", resourceRequestCVId));
            }
            return await WorkScope.GetAsync<ResourceRequestCV>(resourceRequestCVId);
        }

        [HttpPut]
        public async Task<ResourceRequestCV> UpdateResourceRequestCV(ResourceRequestCVDto input)
        {
            var entity = await WorkScope.GetAsync<Entities.ResourceRequestCV>(input.Id);
            ObjectMapper.Map(input, entity);
            await WorkScope.UpdateAsync(entity);
            return entity;
        }

        [HttpDelete]
        public async Task DeleteResourceRequestCV(long resourceRequestCVId)
        {
            await WorkScope.DeleteAsync<ResourceRequestCV>(resourceRequestCVId);
        }

        [HttpPost]
        public async Task<string> UploadCVPathResourceRequestCV([FromForm] UploadCVPathResourceRequestCVDto input)
        {
            if (input.file == null || input.file.Length == 0)
            {
                throw new UserFriendlyException(string.Format("File null"));
            }

            var resourceRequestCV = await WorkScope.GetAsync<ResourceRequestCV>(input.resourceRequestCVId);
            if (resourceRequestCV == null)
            {
                throw new UserFriendlyException(String.Format("Not Found ResourceRequestCV with Id {0} ", input.resourceRequestCVId));
            }

            var filename = DateTimeUtils.NowToyyyyMMddHHmmssfff() + "_" + input.file.FileName.Replace(" ", "_");
            var filePath = await _uploadFileService.UploadCvAsync(input.file, filename);

            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("File Upload Failed");
            }

            resourceRequestCV.CVPath = filePath;
            if (resourceRequestCV.Status == CVStatus.ChualamCV)
            {
                resourceRequestCV.Status = CVStatus.DaLamCV;
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return filePath;

        }
        [HttpPost]
        public async Task<UpdateStatusResultDto> UpdateStatusResourceRequestCV(UpdateResouceRequestCVStatusDto input)
        {
            var resourceRequestCV = await WorkScope.GetAsync<ResourceRequestCV>(input.ResourceRequestCVId);
            resourceRequestCV.Status = input.Status;
            resourceRequestCV.CvStatusId = input.CvStatusId;

            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(resourceRequestCV.ResourceRequestId);
            var getCvStatusId = await WorkScope.GetAll<Entities.CvStatus>().Where(s => s.Name == "Pass").Select(s => s.Id).FirstOrDefaultAsync();
            var result = new UpdateStatusResultDto();
            if (input.CvStatusId == getCvStatusId && !resourceRequest.BillAccountId.HasValue)
            {
                var newBillAcount = new UpdateResourceRequestPlanForBillInfoDto();
                newBillAcount.StartTime = resourceRequestCV.InterviewDate;
                newBillAcount.ResourceRequestId = resourceRequestCV.ResourceRequestId;
                newBillAcount.CVName = resourceRequestCV.CVName;
                newBillAcount.UserId = resourceRequestCV.UserId;
                newBillAcount.CVPath = resourceRequestCV.CVPath;
                var resourceRequestDto = await UpdateBillInfoTemp(newBillAcount);
                result.getResourceRequestDto = resourceRequestDto;
            }

            await WorkScope.UpdateAsync(resourceRequestCV);
            result.updateResouceRequestCVStatus = input;

            return result;
        }

        [HttpPost]
        public async Task<UpdateResourceRequestCVNote> UpdateNoteResourceRequestCV(UpdateResourceRequestCVNote input)
        {
            var resourceRequestCV = await WorkScope.GetAsync<ResourceRequestCV>(input.ResourceRequestCVId);
            resourceRequestCV.Note = input.Note;
            await WorkScope.UpdateAsync(resourceRequestCV);
            return input;
        }

        [HttpPost]
        public async Task<UpdateResouceRequestCVKpiPoint> UpdateKpiPointResourceRequestCV(UpdateResouceRequestCVKpiPoint input)
        {
            var resourceRequestCV = await WorkScope.GetAsync<ResourceRequestCV>(input.ResourceRequestCVId);
            resourceRequestCV.KpiPoint = input.KpiPoint;
            await WorkScope.UpdateAsync(resourceRequestCV);
            return input;
        }

        [HttpPost]
        public async Task<UpdateResouceRequestCVSendCVDate> UpdateSendCVDateResourceRequestCV(UpdateResouceRequestCVSendCVDate input)
        {
            var resourceRequestCV = await WorkScope.GetAsync<ResourceRequestCV>(input.ResourceRequestCVId);
            resourceRequestCV.SendCVDate = input.SendCVDate;
            await WorkScope.UpdateAsync(resourceRequestCV);
            return input;
        }

        [HttpPost]
        public async Task<UpdateResouceRequestCVInterviewTime> UpdateInterviewTimeResourceRequestCV(UpdateResouceRequestCVInterviewTime input)
        {
            var resourceRequestCV = await WorkScope.GetAsync<ResourceRequestCV>(input.ResourceRequestCVId);
            resourceRequestCV.InterviewDate = input.InterviewDate;
            await WorkScope.UpdateAsync(resourceRequestCV);
            return input;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.ResourceRequest)]
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

            if (input.FilterRequestCode != null && input.FilterRequestCode.Any())
            {
                query = query.Where(s => input.FilterRequestCode.Contains(s.Code));
            }
            if (input.FilterRequestStatus != null && input.FilterRequestStatus.Any())
            {
                query = query.Where(s => input.FilterRequestStatus.Contains(s.Status));
            }

            query = _resourceRequestManager.ApplyOrders(query, input.SortParams);
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
        public async Task<List<RequestCodeDto>> GetResourceRequestCode()
        {
            var result = WorkScope.GetAll<ResourceRequest>()
                                  .Where(r => r.Code != null)
                                  .Select(r => new RequestCodeDto { Code = r.Code, Status = r.Status })
                                  .ToList()
                                    .GroupBy(r => new { r.Code, r.Status })
                                    .Select(group => group.First())
                                    .OrderBy(group => group.Status)
                                    .ThenBy(group => group.Code)
                                    .ToList();

            return result;
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
            for (int i = 0; i < input.Quantity; i++)
            {
                var request = ObjectMapper.Map<ResourceRequest>(input);
                request.Quantity = 1;
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
            }

            //SendKomuNotify(model.Name, project.Name, model.Status);
            CurrentUnitOfWork.SaveChanges();

            var listRequestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => createdRequestIds.Contains(s.Id))
                .ToListAsync();

            await notifyToKomu(listRequestDto.FirstOrDefault(), Action.Create, listRequestDto.Count);

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
            resourceRequest.Quantity = 1;
            // update project Id in ProjectUser
            var projectUser = WorkScope.GetAll<ProjectUser>().Where(p => p.ResourceRequestId == input.Id).FirstOrDefault();
            if (projectUser != null)
                projectUser.ProjectId = input.ProjectId;
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

        [HttpPost]
        [AbpAuthorize]
        public async Task<string> UploadCV([FromForm] CVUploadDto input)
        {

            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(input.ResourceRequestId);
            if (resourceRequest == null)
            {
                throw new UserFriendlyException(String.Format("Resource Request Not Found"));
            }
            if (input.file == null || input.file.Length == 0)
            {
                resourceRequest.LinkCV = null;
                return null;
            }
            else
            {
                var filename = input.ResourceRequestId + '_' + input.file.FileName;

                var filePath = await _uploadFileService.UploadCvAsync(input.file, filename);
                if (string.IsNullOrEmpty(filePath))
                {
                    throw new Exception("File Upload Failed");
                }


                resourceRequest.LinkCV = filePath;
                return filePath;
            }
            await CurrentUnitOfWork.SaveChangesAsync();

        }

        [HttpGet]
        public async Task<object> DownloadCVLink(long resourceRequestId)
        {
            var filePath = WorkScope.GetAll<ResourceRequest>()
                .Where(s => s.Id == resourceRequestId)
                .Select(s => s.LinkCV)
                .FirstOrDefault();

            if (filePath == null)
            {
                throw new UserFriendlyException(String.Format("File path not found"));
            }
            var data = await _uploadFileService.DownloadCvLinkAsync(filePath);
            var fileName = FileUtils.GetFileName(filePath);
            return new
            {
                FileName = fileName,
                Data = data
            };
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

            var isGrantedCancelAll = IsGranted(PermissionNames.ResourceRequest_CancelAllRequest);

            if (!isGrantedCancelAll && project.PMId != AbpSession.UserId.Value)
            {
                throw new UserFriendlyException($"Request Id {requestId} is for project that you are NOT PM.");
            }
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.ResourceRequest_CancelAllRequest, PermissionNames.ResourceRequest_CancelMyRequest)]
        public async Task<GetResourceRequestDto> CancelRequest(long requestId)
        {
            await CheckRequestIsForMyProject(requestId, null);
            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(requestId);

            resourceRequest.Status = ResourceRequestStatus.CANCELLED;

            await WorkScope.UpdateAsync(resourceRequest);
            var projectUser = WorkScope.GetAll<ProjectUser>().Where(p => p.ResourceRequestId == requestId).ToList();
            projectUser.ForEach(pu => pu.IsDeleted = true);
            CurrentUnitOfWork.SaveChanges();
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
        [AbpAuthorize(PermissionNames.ResourceRequest_Activate)]
        public async Task<GetResourceRequestDto> ActiveRequest(long requestId)
        {
            await CheckRequestIsForMyProject(requestId, null);
            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(requestId);

            resourceRequest.Status = ResourceRequestStatus.PENDING;

            await WorkScope.UpdateAsync(resourceRequest);

            var requestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == requestId)
                .FirstOrDefaultAsync();

            await notifyToKomu(requestDto, Action.Active, null);
            return requestDto;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<UpdateRequestNoteDto> Description(UpdateRequestNoteDto input)
        {
            var resourceRequest = await WorkScope.GetAsync<ResourceRequest>(input.ResourceRequestId);

            resourceRequest.PMNote = input.Note;

            await WorkScope.UpdateAsync(resourceRequest);

            return input;
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<UpdateRequestNoteDto> Note(UpdateRequestNoteDto input)
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
                    PlanUserInfo = s.ProjectUsers
                                    .Where(s => !s.IsDeleted && s.Status == ProjectUserStatus.Future && s.AllocatePercentage > 0)
                                    .OrderByDescending(x => x.CreationTime).FirstOrDefault()
                }).FirstOrDefaultAsync();

            if (request == default)
            {
                throw new UserFriendlyException("Not found Request with Id " + input.RequestId);
            }

            if (request.PlanUserInfo != null)
            {
                request.PlanUserInfo.Note = string.Empty;
                await _resourceManager.ConfirmJoinProject(request.PlanUserInfo.Id, input.StartTime, true);
            }

            request.Request.Status = ResourceRequestStatus.DONE;
            request.Request.TimeDone = DateTimeUtils.GetNow();
            request.Request.BillStartDate = input.BillStartTime;
            await WorkScope.UpdateAsync(request.Request);

            // add user in cv column to project user bill table
            if (request.Request.BillAccountId != null)
            {
                var existedPUB = await WorkScope.GetAll<ProjectUserBill>()
                   .Where(x => x.ProjectId == request.Request.ProjectId && x.UserId == request.Request.BillAccountId)
                   .FirstOrDefaultAsync();
                if (existedPUB == null)
                {
                    existedPUB = await WorkScope.InsertAsync(new ProjectUserBill
                    {
                        UserId = request.Request.BillAccountId ?? default,
                        StartTime = input.BillStartTime ?? default,
                        ProjectId = request.Request.ProjectId,
                        isActive = true,
                        AccountName = request.Request.CVName,
                        LinkCV = request.Request.LinkCV,
                    });
                    CurrentUnitOfWork.SaveChanges();
                }
                if (request.PlanUserInfo != null)
                {
                    var newLinkedResource = new LinkedResource
                    {
                        UserId = request.PlanUserInfo.UserId,
                        ProjectUserBillId = existedPUB.Id,
                    };

                    await WorkScope.InsertAsync(newLinkedResource);
                }
            }
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
                    return "has **created** a resource request:";

                case Action.Cancel:
                    return "has **cancelled** the resource request:";

                case Action.Plan:
                    return "has **planned** for the resource request:";

                case Action.Done:
                    return "has **done** the resource request:";

                case Action.Active:
                    return "has **actived** the resource request:";
            }
            return "";
        }

        private void SendKomu(StringBuilder komuMessage)
        {
            _komuService.NotifyToChannel(new KomuMessage
            {
                CreateDate = DateTimeUtils.GetNow(),
                Message = komuMessage.ToString(),
            },
           ChannelTypeConstant.PM_CHANNEL);
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
                    ResourceRequestId = projectUser.ResourceRequestId.GetValueOrDefault()
                };
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PlanUserInfoDto> CreateResourceRequestPlan(ResourceRequestPlanDto input)
        {
            var request = WorkScope.Get<ResourceRequest>(input.ResourceRequestId);

            var activeReportId = await _resourceManager.GetActiveReportId();

            var projectUser = new ProjectUser()
            {
                UserId = input.UserId ?? default,
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

            WorkScope.Insert(projectUser);
            CurrentUnitOfWork.SaveChanges();

            var requestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == input.ResourceRequestId)
                .FirstOrDefaultAsync();

            await notifyToKomu(requestDto, Action.Plan, null);

            return requestDto.PlanUserInfo;
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.ResourceRequest_CreateBillResourceForRequest)]
        public async Task<GetResourceRequestDto> UpdateBillInfoTemp(UpdateResourceRequestPlanForBillInfoDto input)
        {
            var request = WorkScope.Get<ResourceRequest>(input.ResourceRequestId);

            request.BillAccountId = input.UserId;
            request.BillStartDate = input.StartTime;
            request.CVName = input.CVName;
            request.LinkCV = input.CVPath;
            await WorkScope.UpdateAsync(request);

            var isAlreadyHaveResource = WorkScope.GetAll<ProjectUser>()
                          .Where(s => s.ResourceRequestId == input.ResourceRequestId && s.Status == ProjectUserStatus.Future && s.AllocatePercentage > 0)
                          .Any();

            if (!isAlreadyHaveResource && request.BillAccountId.HasValue)
            {
                var activeReportId = await _resourceManager.GetActiveReportId();

                var pu = new ProjectUser()
                {
                    UserId = input.UserId ?? default,
                    ProjectId = request.ProjectId,
                    ProjectRole = input.ProjectRole,
                    AllocatePercentage = 100,
                    StartTime = input.StartTime.HasValue ? input.StartTime.Value : DateTime.Now.Date,
                    Status = ProjectUserStatus.Future,
                    ResourceRequestId = input.ResourceRequestId,
                    PMReportId = activeReportId,
                    IsPool = false,
                    Note = "Planned for resource request Id " + input.ResourceRequestId
                };
                WorkScope.Insert(pu);

            }
            await CurrentUnitOfWork.SaveChangesAsync();

            var requestDto = await _resourceRequestManager.IQGetResourceRequest()
                .Where(s => s.Id == input.ResourceRequestId)
                .FirstOrDefaultAsync();

            await notifyToKomu(requestDto, Action.Plan, null);

            return requestDto;
        }

        [HttpDelete]
        [AbpAuthorize]
        public async Task RemoveResourceRequestPlan(long requestId)
        {
            await _resourceRequestManager.RemoveResourceRequestPlan(requestId);
        }

        [HttpPost]
        [AbpAuthorize]
        public async Task<PlanUserInfoDto> UpdateResourceRequestPlan(ResourceRequestPlanDto input)
        {
            var projectUser = WorkScope.Get<ProjectUser>(input.ProjectUserId);

            projectUser.UserId = input.UserId ?? default;
            projectUser.StartTime = input.StartTime;
            projectUser.ProjectRole = input.ProjectRole;
            projectUser.Status = ProjectUserStatus.Future;

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
            Done = 4,
            Active = 5
        }
    }
}