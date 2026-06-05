using Abp.Authorization;
using Abp.Configuration;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using NccCore.Uitls;
using Newtonsoft.Json;
using ProjectManagement.APIs.ProjectUserOnboarding.Dto;
using ProjectManagement.Authorization;
using ProjectManagement.Constants;
using ProjectManagement.Entities;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ResourceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserOnboarding
{
    [AbpAuthorize]
    public class ProjectUserOnboardingAppService : ProjectManagementAppServiceBase, IProjectUserOnboardingAppService
    {
        private readonly ResourceManager _resourceManager;
        private ISettingManager _settingManager;
        //private KomuService _komuService;
        private static IConfiguration _appConfiguration;

        public ProjectUserOnboardingAppService(
            IConfiguration appConfiguration,
            //KomuService komuService,
            ResourceManager resourceManager,
            ISettingManager settingManager) : base()
        {
            _resourceManager = resourceManager;
            //_komuService = komuService;
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
                        //await SendConfirmationRequestToMember(input.ProjectUserId);
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

            //await SendConfirmationRequestToMember(projectUserId);
        }

        //#region API Helper methods

        /*private async Task SendConfirmationRequestToMember(long projectUserId)
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
            sbMessage.AppendLine("🗣️ **User confirmation:**");
            sbMessage.AppendLine("> *Bạn xác nhận đã đọc kỹ và hoàn thành đầy đủ các mục yêu cầu trong Onboarding Checklist.*");
            await _komuService.NotifyToKomuUserAwait(new KomuMessage
            {
                UserName = "tu.lecam",
                Message = sbMessage.ToString(),
                CreateDate = DateTimeUtils.GetNow(),
            });

        }
        #endregion */
    }
}
