using Abp.Authorization;
using Abp.Configuration;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using NccCore.Extension;
using NccCore.Paging;
using NccCore.Uitls;
using Newtonsoft.Json;
using ProjectManagement.APIs.ProjectUserOnboarding.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Constants;
using ProjectManagement.Entities;
using ProjectManagement.Manager.OffboardUserManager.Dto;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ResourceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserOnboarding
{
    [AbpAuthorize]
    public class ProjectUserOnboardingAppService : ProjectManagementAppServiceBase, IProjectUserOnboardingAppService
    {
        private readonly ResourceManager _resourceManager;
        private ISettingManager _settingManager;
        private KomuService _komuService;
        private static IConfiguration _appConfiguration;

        public ProjectUserOnboardingAppService(
            IConfiguration appConfiguration,
            KomuService komuService,
            ResourceManager resourceManager,
            ISettingManager settingManager) : base()
        {
            _resourceManager = resourceManager;
            _komuService = komuService;
            _settingManager = settingManager;
            _appConfiguration = appConfiguration;
        }


        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_OnboardingChecklist_ForceDone)]
        public async Task ForceDone(long projectUserId)
        {
            try
            {
                var onboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>()
                    .Include(x => x.ProjectUserOnboardingDetails)
                    .FirstOrDefaultAsync(x => x.ProjectUserId == projectUserId);

                if (onboarding == null)
                {
                    onboarding = new Entities.ProjectUserOnboarding
                    {
                        ProjectUserId = projectUserId,
                        ProjectUserOnboardingDetails = new List<ProjectUserOnboardingDetail>()
                    };
                    onboarding.Id = await WorkScope.InsertAndGetIdAsync(onboarding);
                }

                onboarding.Status = ProjectUserOnboardingStatus.Done;
                onboarding.ConfirmedTime = DateTime.Now;

                var templates = await WorkScope.GetAll<Entities.OnboardingChecklist>().ToListAsync();

                foreach (var t in templates)
                {
                    var detail = onboarding.ProjectUserOnboardingDetails
                                           .FirstOrDefault(d => d.OnboardingChecklistId == t.Id);

                    if (detail != null)
                    {
                        detail.IsChecked = true;
                    }
                    else
                    {
                        onboarding.ProjectUserOnboardingDetails.Add(new ProjectUserOnboardingDetail
                        {
                            OnboardingChecklistId = t.Id,
                            IsChecked = true
                        });
                    }
                }

                await WorkScope.UpdateAsync(onboarding);
            }
            catch (Exception ex)
            {
                Logger.Error("Force Done failed: " + ex.Message);
                throw new UserFriendlyException("Failed to force status to Done.");
            }
        }

        [HttpPut]
        public async Task UpdateOnboardHistoryNote(UpdateOnboardHistoryNoteDto input)
        {
            var onboarding = await WorkScope.GetAsync<Entities.ProjectUserOnboarding>(input.ProjectUserOnboardingId);

            if (onboarding == null)
            {
                throw new UserFriendlyException("Onboarding history not found");
            }

            onboarding.Note = input.Note;
            await WorkScope.UpdateAsync(onboarding);
        }

        [HttpPost]
        public async Task<GridResult<OnboardHistoryDto>> GetAllOnboardHistory(InputGetAllOnboardHistoryDto input)
        {
            var query = WorkScope.GetAll<Entities.ProjectUserOnboarding>()
                .AsNoTracking()
                .Select(x => new OnboardHistoryDto
                {
                    Id = x.Id,
                    ProjectUserId = x.ProjectUserId,
                    UserId = x.ProjectUser.UserId,
                    ProjectId = x.ProjectUser.ProjectId,
                    EmailAddress = x.ProjectUser.User.EmailAddress,
                    AvatarPath = x.ProjectUser.User.AvatarPath,
                    UserType = x.ProjectUser.User.UserType,
                    Branch = x.ProjectUser.User.BranchOld,
                    FullName = x.ProjectUser.User.Name + " " + x.ProjectUser.User.Surname,
                    BranchColor = x.ProjectUser.User.Branch != null ? x.ProjectUser.User.Branch.Color : null,
                    BranchDisplayName = x.ProjectUser.User.Branch != null ? x.ProjectUser.User.Branch.DisplayName : null,
                    PositionId = x.ProjectUser.User.PositionId,
                    PositionName = x.ProjectUser.User.Position != null ? x.ProjectUser.User.Position.Name : null,
                    PositionColor = x.ProjectUser.User.Position != null ? x.ProjectUser.User.Position.Color : null,
                    UserLevel = x.ProjectUser.User.UserLevel,
                    ProjectName = x.ProjectUser.Project != null ? x.ProjectUser.Project.Name : null,
                    ProjectType = x.ProjectUser.Project != null ? x.ProjectUser.Project.ProjectType : 0,
                    ProjectCode = x.ProjectUser.Project != null ? x.ProjectUser.Project.Code : null,
                    ProjectRole = x.ProjectUser.ProjectRole,
                    ProjectPM = x.ProjectUser.Project.PM != null ? x.ProjectUser.Project.PM.FullName : null,
                    PMEmail = x.ProjectUser.Project.PM != null ? x.ProjectUser.Project.PM.EmailAddress : null,
                    PMId = x.ProjectUser.Project.PMId,
                    Status = x.Status,
                    Note = x.Note,
                });

            if (input.ProjectId.HasValue && input.ProjectId.Value > 0)
            {
                query = query.Where(x => x.ProjectId == input.ProjectId.Value);
            }

            if (input.Status.HasValue)
            {
                query = query.Where(x => x.Status == input.Status.Value);
            }

            if (input.PMId.HasValue && input.PMId.Value > 0)
            {
                query = query.Where(x => x.PMId == input.PMId.Value);   
            }

            if (!string.IsNullOrEmpty(input.SearchText))
            {
                query = query.Where(x =>
                    x.EmailAddress.ToLower().Contains(input.SearchText.ToLower()) ||
                    x.FullName.ToLower().Contains(input.SearchText.ToLower()));
            }

            var total = await query.CountAsync();
            var list = await query.TakePage(input).ToListAsync();

            return new GridResult<OnboardHistoryDto>(list, total);
        }

        [HttpGet]
        public async Task<GetOnboardingDto> GetOnboardingInfor(long projectUserId)
        {
            try
            {
                var templates = await WorkScope.GetAll<Entities.OnboardingChecklist>()
                    .OrderBy(x => x.Order)
                    .ToListAsync();

                var onboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>()
                    .Include(x => x.ProjectUserOnboardingDetails)
                    .FirstOrDefaultAsync(x => x.ProjectUserId == projectUserId);

                var checklistResult = templates.Select(t => new OnboardingChecklistItemDto
                {
                    Id = t.Id,
                    Label = t.Label,

                    Details = t.DetailsJson,

                    IsChecked = onboarding != null &&
                                onboarding.ProjectUserOnboardingDetails != null &&
                                onboarding.ProjectUserOnboardingDetails.Any(d => d.OnboardingChecklistId == t.Id && d.IsChecked)
                }).ToList();

                if (onboarding == null)
                {
                    return new GetOnboardingDto
                    {
                        ProjectUserId = projectUserId,
                        Status = ProjectUserOnboardingStatus.NotStarted,
                        Checklist = checklistResult
                    };
                }

                return new GetOnboardingDto
                {
                    ProjectUserId = projectUserId,
                    Status = onboarding.Status,
                    Checklist = checklistResult
                };
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to retrieve onboarding information: " + ex.Message, ex);
                throw new UserFriendlyException("Failed to retrieve onboarding information.");
            }
        }


        [HttpPost]
        public async Task<long> OnboardingUser(AddOnboardingDto input)
        {
            try
            {
                var projectUserOnboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>()
                    .Include(x => x.ProjectUserOnboardingDetails)
                    .FirstOrDefaultAsync(x => x.ProjectUserId == input.ProjectUserId);

                if (projectUserOnboarding == null)
                {
                    projectUserOnboarding = new Entities.ProjectUserOnboarding
                    {
                        ProjectUserId = input.ProjectUserId,
                        ProjectUserOnboardingDetails = new List<ProjectUserOnboardingDetail>()
                    };
                }
                foreach (var item in input.Checklist)
                {
                    var existingDetail = projectUserOnboarding.ProjectUserOnboardingDetails
                        .FirstOrDefault(d => d.OnboardingChecklistId == item.Id);

                    if (existingDetail != null)
                    {
                        existingDetail.IsChecked = item.IsChecked;
                    }
                    else
                    {
                        projectUserOnboarding.ProjectUserOnboardingDetails.Add(new Entities.ProjectUserOnboardingDetail
                        {
                            OnboardingChecklistId = item.Id,
                            IsChecked = item.IsChecked
                        });
                    }
                }

                bool isAllChecked = input.Checklist.All(x => x.IsChecked);

                if (isAllChecked)
                {
                    if (projectUserOnboarding.Status != ProjectUserOnboardingStatus.Done &&
                        projectUserOnboarding.Status != ProjectUserOnboardingStatus.Pending)
                    {
                        projectUserOnboarding.Status = ProjectUserOnboardingStatus.Pending;
                        projectUserOnboarding.SentRequestTime = DateTime.Now;
                        await SendConfirmationRequestToMember(input.ProjectUserId, input.Checklist);
                    }
                }
                else
                {
                    if (projectUserOnboarding.Status != ProjectUserOnboardingStatus.Done)
                    {
                        projectUserOnboarding.Status = ProjectUserOnboardingStatus.InProgress;
                    }
                }

                return await WorkScope.InsertOrUpdateAndGetIdAsync(projectUserOnboarding);

            }
            catch (Exception)
            {
                throw new UserFriendlyException("Failed to save onboarding checklist.");
            }

        }


        [HttpPost]
        public async Task Remind(long projectUserId)
        {
            var onboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>()
                 .Include(x => x.ProjectUserOnboardingDetails)
                 .ThenInclude(d => d.OnboardingChecklist)
                 .FirstOrDefaultAsync(x => x.ProjectUserId == projectUserId);

            bool canRemind = onboarding != null &&
                             (onboarding.Status == ProjectUserOnboardingStatus.PendingEmployee ||
                              onboarding.Status == ProjectUserOnboardingStatus.Pending);

            if (!canRemind)
            {
                throw new UserFriendlyException("Cannot send reminder. User is not in a Pending state.");
            }

            onboarding.SentRequestTime = DateTime.Now;
            onboarding.Status = ProjectUserOnboardingStatus.PendingEmployee;
            await WorkScope.UpdateAsync(onboarding);
            var checklist = onboarding.ProjectUserOnboardingDetails.Select(d => new OnboardingChecklistItemDto
            {
                Id = d.OnboardingChecklistId,
                Label = d.OnboardingChecklist.Label,
                Details = d.OnboardingChecklist.DetailsJson,
                IsChecked = d.IsChecked
            }).ToList();
            await SendConfirmationRequestToMember(projectUserId, checklist);
        }

        [HttpDelete]
        public async Task Delete(long onboardHistoryId)
        {
            var onboard = await WorkScope.GetAsync<Entities.ProjectUserOnboarding>(onboardHistoryId);
            if (onboard == null)
                throw new UserFriendlyException("Onboard History not exist");
            await WorkScope.DeleteAsync(onboard);
        }

        #region API Helper methods
        private async Task SendConfirmationRequestToMember(long projectUserId, List<OnboardingChecklistItemDto> Checklist)
        {
            var projectUser = await WorkScope.GetAll<ProjectUser>()
                            .Include(x => x.User)
                            .Include(x => x.Project)
                            .FirstOrDefaultAsync(x => x.Id == projectUserId);

            if (projectUser == null)
                throw new UserFriendlyException("Project user not found.");

            var sbMessage = new StringBuilder();
            sbMessage.AppendLine($"✅**ONBOARDING CONFIRM**");
            sbMessage.AppendLine($"Project: **{projectUser.Project.Name}**");
            sbMessage.AppendLine("------------------------------------------------");

            if (Checklist != null && Checklist.Count > 0)
            {
                sbMessage.AppendLine("📋 **Onboarding Checklist:**");
                foreach (var item in Checklist)
                {
                    sbMessage.AppendLine($"- **{item.Label}**");

                    if (!string.IsNullOrWhiteSpace(item.Details))
                    {
                        string cleanDetails = WebUtility.HtmlDecode(item.Details);

                        cleanDetails = Regex.Replace(cleanDetails, @"<br\s*/?>", "\n", RegexOptions.IgnoreCase);
                        cleanDetails = Regex.Replace(cleanDetails, @"</p>", "\n", RegexOptions.IgnoreCase);
                        cleanDetails = Regex.Replace(cleanDetails, @"<[^>]+>", string.Empty);
                        cleanDetails = cleanDetails.Replace("&nbsp;", " ").Trim();

                        var lines = cleanDetails.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (var line in lines)
                        {
                            var trimmedLine = line.Trim();
                            if (!string.IsNullOrEmpty(trimmedLine))
                            {
                                sbMessage.AppendLine($"  > {trimmedLine}");
                            }
                        }
                    }
                }
                sbMessage.AppendLine("------------------------------------------------");
            }

            sbMessage.AppendLine("🗣️ **User confirmation:**");
            sbMessage.AppendLine("> *Bạn xác nhận đã đọc kỹ và hoàn thành đầy đủ các mục yêu cầu trong Onboarding Checklist.*");

            var komuUserName = projectUser.User.UserName?.Split('@')[0];
            await _komuService.NotifyToKomuUserAwait(new KomuMessage
            {
                UserName = komuUserName,
                Message = sbMessage.ToString(),
                CreateDate = DateTimeUtils.GetNow(),
            });
        }
        #endregion
    }
}
