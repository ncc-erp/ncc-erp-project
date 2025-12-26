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
        [AbpAuthorize(PermissionNames.Admin)]
        public async Task ForceDone(long projectUserId)
        {
            try
            {
                var projectUserOnboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>().FirstOrDefaultAsync(x => x.ProjectUserId == projectUserId);
                if (projectUserOnboarding != null)
                {
                    projectUserOnboarding.Status = ProjectUserOnboardingStatus.Done;
                    projectUserOnboarding.ConfirmedTime = DateTime.Now;
                }
                await WorkScope.UpdateAsync(projectUserOnboarding);
            }
            catch (Exception)
            {
                throw new UserFriendlyException("Failed to force status to Done.");
            }
           
        }


        [HttpGet]
        public async Task<GetOnboardingDto> GetOnboardingInfor(long projectUserId)
        {
            try
            {
                var onboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>().FirstOrDefaultAsync(x => x.ProjectUserId == projectUserId);
          
                if (onboarding == null)
                {
                    return new GetOnboardingDto
                    {
                        ProjectUserId = projectUserId,
                        Status = ProjectUserOnboardingStatus.NotStarted,
                        Checklist = GetDefaultChecklistTemplate()
                    };
                }

                return new GetOnboardingDto
                {
                    ProjectUserId = projectUserId,
                    Status = onboarding.Status,
                    Checklist = JsonConvert.DeserializeObject<List<OnboardingChecklistItemDto>>(onboarding.ChecklistJson)
                };
            }
            catch (Exception)
            {
                throw new UserFriendlyException("Failed to retrieve onboarding information.");
            }
        }


        [HttpPost]
        public async Task<long> OnboardingUser(AddOnboardingDto input)
        {
            try
            {
                var projectUserOnboarding = await WorkScope.GetAll<Entities.ProjectUserOnboarding>().FirstOrDefaultAsync(x => x.ProjectUserId == input.ProjectUserId);

                if (projectUserOnboarding == null)
                {
                    projectUserOnboarding = new Entities.ProjectUserOnboarding { ProjectUserId = input.ProjectUserId };
                }

                bool isAllChecked = input.Checklist.All(x => x.IsChecked);

                projectUserOnboarding.ChecklistJson = JsonConvert.SerializeObject(input.Checklist);

                if (isAllChecked)
                {
                    projectUserOnboarding.Status = ProjectUserOnboardingStatus.Pending;
                    projectUserOnboarding.SentRequestTime = DateTime.Now;
                    await SendConfirmationRequestToMember(input.ProjectUserId);
                }
                else
                {
                    projectUserOnboarding.Status = ProjectUserOnboardingStatus.InProgress;
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

            await SendConfirmationRequestToMember(projectUserId);
        }

        #region API Helper methods

        private async Task SendConfirmationRequestToMember(long projectUserId)
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
                UserName = "chinh.vuquang",
                Message = sbMessage.ToString(),
                CreateDate = DateTimeUtils.GetNow(),
            });

        }

        private List<OnboardingChecklistItemDto> GetDefaultChecklistTemplate()
        {
            var checklist = _appConfiguration.GetSection("OnboardingSettings:DefaultChecklist")
                                          .Get<List<OnboardingChecklistItemDto>>();
            if (checklist?.Count > 0)
            {
                return checklist;
            }

            return new List<OnboardingChecklistItemDto>
            {
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_1",
                    Label = "Checklist 1: PM tạo meeting onboarding (Giới thiệu member, Daily meeting)",
                    Details = new List<string>
                    {
                        "Việc giới thiệu new member, all old member thường ở daily meeting",
                        "PM giới thiệu hoặc member tự giới thiệu các thông tin: tên, tuổi, văn phòng, vị trí (dev fe, dev be, dev fullstack, qa, lead fe, ..)"
                    }
                },
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_2",
                    Label = "Checklist 2: PM nói expect đối với new member (Thái độ, cách làm việc)",
                    Details = new List<string>
                    {
                        "Mong muốn nhân viên có thái độ, cách làm việc, mức độ perform như thế nào,..."
                    }
                },
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_3",
                    Label = "Checklist 3: Yêu cầu member đọc kỹ 'Các lỗi ngớ ngẩn gây hậu quả nghiêm trọng'",
                    Details = new List<string>
                    {
                        "Cái này rất quan trọng, PM cần yêu cầu member dành ra tối thiểu 30 phút để đọc kỹ.",
                        "Điều nào không hiểu thì phải hỏi lại ngay.",
                        "PM cần hỏi lại member xem đã hiểu hết chưa, còn chỗ nào thắc mắc không.",
                        "Chỗ nào member chưa hiểu thì PM cần giải thích cho đến khi member hiểu rõ."
                    }
                },
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_4",
                    Label = "Checklist 4: PM nói rõ về Account, Stakeholder (Passbolt, Internal/External teams)",
                    Details = new List<string>
                    {
                        "PM share thông tin account trên passbolt cho member",
                        "Nói rõ cho member: mình đại diện cho bên nào",
                        "PM share thông tin internal team, external team (trên ops) cho member"
                    }
                },
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_5",
                    Label = "Checklist 5: PM phổ biến quy trình làm task (Jira, Notion, Status flow)",
                    Details = new List<string>
                    {
                        "Nhận task ở đâu, từ ai",
                        "Kéo task (todo,inprogress, PR review, ....)",
                        "Clear/confirm req của task trên đầu (notion/Jira, ...)"
                    }
                },
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_6",
                    Label = "Checklist 6: PM phổ biến quy trình Git flow của dự án",
                    Details = new List<string>
                    {
                        "Phần này PM cần đưa lên OPS và gửi link cho member đọc"
                    }
                },
                new OnboardingChecklistItemDto
                {
                    Key = "checklist_7",
                    Label = "Checklist 7: PM assign Mentor cho new member (Bắt buộc với Intern)",
                    Details = new List<string>
                    {
                        "Intern bắt buộc phải có mentor"
                    }
                }
            };
        }
        #endregion
    }
}
