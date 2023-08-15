using Abp.Authorization;
using Abp.UI;
using Amazon.Runtime.Internal.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NccCore.Helper;
using NccCore.IoC;
using NccCore.Uitls;
using ProjectManagement.APIs.HRMv2.Dto;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Configuration;
using ProjectManagement.Constants;
using ProjectManagement.Entities;
using ProjectManagement.NccCore.Helper;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using ProjectManagement.Services.ResourceManager;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.HRMv2
{
    public class Hrmv2AppService : ProjectManagementAppServiceBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private KomuService _komuService;
        private ResourceManager _resourceManager;
        public Hrmv2AppService(IHttpContextAccessor httpContextAccessor, KomuService komuService, ResourceManager resourceManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _komuService = komuService;
            _resourceManager = resourceManager;

        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task UpdateAvatarFromHrm(UpdateAvatarDto input)
        {
            CheckSecurityCode();
            if (string.IsNullOrEmpty(input.AvatarPath))
            {
                Logger.Error($"user with {input.AvatarPath} is null or empty");
                return;
            }
            var user = await GetUserByEmailAsync(input.EmailAddress);

            if (user == null)
            {
                Logger.Error($"not found user with email {input.EmailAddress}");
                return;
            }

            user.AvatarPath = input.AvatarPath;
            await WorkScope.UpdateAsync(user);
        }
        private void CheckSecurityCode()
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"].ToString();
            if (secretCode == securityCodeHeader)
                return;

            throw new UserFriendlyException($"SecretCode does not match: ProjectCode: {secretCode.Substring(secretCode.Length - 3)} != {securityCodeHeader}");
        }
        private async Task<ProjectManagement.Entities.Branch> GetBranchByCode(string code)
        {
            var branch = await WorkScope.GetAll<ProjectManagement.Entities.Branch>().Where(s => s.Code == code).FirstOrDefaultAsync();
            if (branch == default)
            {
                branch = await WorkScope.GetAll<ProjectManagement.Entities.Branch>().FirstOrDefaultAsync();
            }
            return branch;
        }
        private long GetPositionIdByCode(string code)
        {
            var positionDevId = WorkScope.GetAll<Position>()
                                         .Where(s => s.Code.ToLower().Trim() == "dev")
                                         .Select(s => s.Id)
                                        .FirstOrDefault();
            if (code == null)
                return positionDevId;

            var positionId = WorkScope.GetAll<Position>()
                                      .Where(s => s.Code.ToLower().Trim() == code.ToLower().Trim())
                                      .Select(s => s.Id)
                                      .FirstOrDefault();

            if (positionId == default && positionDevId != default)
                return positionDevId;
            return positionId;
        }
        public async Task AddUserSkills(long userId, List<string> skillNames)
        {

            if (skillNames == null || skillNames.Count == 0)
            {
                return;
            }

            var skillIds = WorkScope.GetAll<Skill>()
                                  .Where(s => skillNames.Contains(s.Name))
                                  .Select(s => s.Id)
                                  .ToList();

            List<UserSkill> listToAdd = skillIds.Select(skillId => new UserSkill
            {
                UserId = userId,
                SkillId = skillId
            }).ToList();

            await WorkScope.InsertRangeAsync(listToAdd);
        }
        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<User> CreateUserByHRM(CreateUpdateUserFromHRMV2Dto input)
        {
            CheckSecurityCode();
            var existUser = WorkScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim());

            if (existUser.Any())
            {
                throw new UserFriendlyException($"failed to create user from HRM, user with email {input.EmailAddress} is already exist");
            }
            var branch = await GetBranchByCode(input.BranchCode);
            var positionId = GetPositionIdByCode(input.PositionCode);
            var user = new User
            {
                UserName = input.EmailAddress.ToLower(),
                Name = input.Name,
                Surname = input.Surname,
                EmailAddress = input.EmailAddress,
                NormalizedEmailAddress = input.EmailAddress.ToUpper(),
                NormalizedUserName = input.EmailAddress.Replace("@ncc.asia", "").ToLower(),
                UserType = input.Type,
                UserLevel = input.Level,
                IsActive = true,
                //Password = input.Password,
                BranchId = branch.Id,
                PositionId = positionId,
            };

            user.Password = RandomPasswordHelper.CreateRandomPassword(8);
            var userId = await WorkScope.InsertAndGetIdAsync(user);

            await AddUserSkills(userId, input.SkillNames);

            var userName = UserHelper.GetUserName(user.EmailAddress);
            var messageToGeneral = $"Welcome **{userName}** [{branch.DisplayName}] to **NCC**";


            _komuService.NotifyToChannel(new KomuMessage
            {
                UserName = userName,
                Message = messageToGeneral,
                CreateDate = DateTimeUtils.GetNow(),
            }, ChannelTypeConstant.GENERAL_CHANNEL);

            var messageToPM = $"HR has onboarded: **{userName}** [{branch.DisplayName}](**{CommonUtil.UserLevelName(user.UserLevel)}**)";

            _komuService.NotifyToChannel(new KomuMessage
            {
                UserName = userName,
                Message = messageToPM,
                CreateDate = DateTimeUtils.GetNow(),
            }, ChannelTypeConstant.PM_CHANNEL);

            return user;
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task UpdateUserFromHRM(CreateUpdateUserFromHRMV2Dto input)
        {
            CheckSecurityCode();
            var user = await WorkScope.GetAll<User>()
                .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
                .FirstOrDefaultAsync();
            var positionId = GetPositionIdByCode(input.PositionCode);
            if (user != null)
            {
                var branch = await GetBranchByCode(input.BranchCode);
                user.UserName = input.EmailAddress;
                user.Name = input.Name;
                user.Surname = input.Surname;
                user.EmailAddress = input.EmailAddress;
                user.UserType = input.Type;
                user.UserLevel = input.Level;
                user.BranchId = branch.Id;
                user.PositionId = positionId;
            }
            await WorkScope.UpdateAsync(user);
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<string> ConfirmUserQuit(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();
            var user = WorkScope.GetAll<User>()
              .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
              .FirstOrDefault();

            if (user == default)
            {
                return "PROJECT tool: Not found user with email " + input.EmailAddress.ToLower().Trim();
            }

            //Update user status
            await IsUpdateUserStatus(user, false);

            var activeReportId = await _resourceManager.GetActiveReportId();
            string note = "Tạm nghỉ từ HRM Tool";
           
            // Join dự án nghỉ việc dưới dạng Official
            var inputToJoinQuitProject = new InputUpdateUserIntoProjectDto()
            {
                UserId = user.Id,
                Note = note,
                ActiveReportId = activeReportId,
                StartTime = input.DateAt,
                IsPool = false,
            };
            await JoinOrUpdateUserIntoQuitProject(inputToJoinQuitProject);

            // Ra khỏi các dự án đang làm việc
            var employee = new KomuUserInfoDto
            {
                FullName = user.FullName,
                UserId = user.Id,
                KomuUserId = user.KomuUserId,
                UserName = user.UserName
            };
            var sb = await _resourceManager.ReleaseUserFromAllWorkingProjectsByHRM(employee, activeReportId, "Nghỉ việc từ HRM Tool");

            //Thông báo cho komu
            sb.AppendLine($"{employee.KomuAccountInfo} quited job on {DateTimeUtils.ToString(input.DateAt)}");
            sb.AppendLine("");

            await SendKomu(sb);
            return sb.ToString();

        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<string> ConfirmUserPause(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();
            var user = WorkScope.GetAll<User>()
              .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
              .FirstOrDefault();

            if (user == default)
            {
                return "PROJECT tool: Not found user with email " + input.EmailAddress.ToLower().Trim();
            }

            //Update user status
            await IsUpdateUserStatus(user, true);

            var activeReportId = await _resourceManager.GetActiveReportId();
            string note = "Tạm nghỉ từ HRM Tool";

            // Join dự án tạm nghỉ dạng official
            var inputToJoinQuitProject = new InputUpdateUserIntoProjectDto()
            {
                UserId = user.Id,
                Note = note,
                ActiveReportId = activeReportId,
                StartTime = input.DateAt,
                IsPool = false,
            };

            
            await JoinOrUpdateUserIntoPauseProject(inputToJoinQuitProject);

            // Ra khỏi các dự án đang làm việc
            var employee = new KomuUserInfoDto
            {
                FullName = user.FullName,
                UserId = user.Id,
                KomuUserId = user.KomuUserId,
                UserName = user.UserName
            };
            var sb = await _resourceManager.ReleaseUserFromAllWorkingProjectsByHRM(employee, activeReportId, note);

           
            // Gửi thông báo cho komu
            sb.AppendLine($"{employee.KomuAccountInfo} pause on {DateTimeUtils.ToString(input.DateAt)}");
            sb.AppendLine("");

            await SendKomu(sb);
            return sb.ToString();

        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<string> ConfirmUserMaternityLeave(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();

            var user = WorkScope.GetAll<User>()
              .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
              .FirstOrDefault();

            if (user == default)
            {
                return "PROJECT tool: Not found user with email " + input.EmailAddress.ToLower().Trim();
            }

            //Update user status
            await IsUpdateUserStatus(user, true);

            var activeReportId = await _resourceManager.GetActiveReportId();
            string note = "Nghỉ sinh từ HRM Tool";
            


            // Join dự án nghỉ sinh
            var inputToJoinMaternityLeave = new InputUpdateUserIntoProjectDto()
            {
                UserId = user.Id,
                Note = note,
                ActiveReportId = activeReportId,
                StartTime = input.DateAt,
                IsPool = true,
                
            };
            var pu = await JoinOrUpdateUserIntoMaternityLeaveProject(inputToJoinMaternityLeave);
            // Ra khỏi các dự án đang làm việc
            var employee = new KomuUserInfoDto
            {
                FullName = user.FullName,
                UserId = user.Id,
                KomuUserId = user.KomuUserId,
                UserName = user.UserName
            };
            var sb = await _resourceManager.ReleaseUserFromAllWorkingProjectsByHRM(employee, activeReportId, note);
            //Gửi thông báo cho komu
            sb.AppendLine($"{employee.KomuAccountInfo} was **maternity leave** from {DateTimeUtils.ToString(pu.StartTime)}");
            sb.AppendLine("");

            await SendKomu(sb);
            return sb.ToString();
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<string> ConfirmUserBackToWork(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();

            var user = WorkScope.GetAll<User>()
              .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
              .FirstOrDefault();
            if (user == default)
            {
                return "PROJECT tool: Not found user with email " + input.EmailAddress.ToLower().Trim();
            }
            //Update user status
            await IsUpdateUserStatus(user, true);

            long activeReportId = await _resourceManager.GetActiveReportId();
            string note = "User back to work from HRM tool";
            var employee = new KomuUserInfoDto
            {
                FullName = user.FullName,
                UserId = user.Id,
                KomuUserId = user.KomuUserId,
                UserName = user.UserName
            };
            var sb = await _resourceManager.ReleaseUserFromAllWorkingProjectsByHRM(employee, activeReportId, note);

            sb.AppendLine($"{employee.KomuAccountInfo} was **back to work** from {DateTimeUtils.ToString(input.DateAt)}");
            sb.AppendLine("");

            await SendKomu(sb);
            return sb.ToString();
        }


        public async Task IsUpdateUserStatus(User user, bool IsActive)
        {
            user.IsActive = IsActive;
            await WorkScope.UpdateAsync(user);
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<string> PlanUserQuit(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();
            bool IsPool = false;
            await PlanUserFromHRMToProject(input, AppConsts.CHO_NGHI_PROJECT_CODE, IsPool);
            return "Successful plan user to project CHO_NGHI";
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task<string> PlanUserPause(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();
            bool IsPool = false;
            await PlanUserFromHRMToProject(input, AppConsts.TAM_NGHI_PROJECT_CODE, IsPool);
            return "Successful plan user to project CHO_NGHI";
        }

        [HttpPost]
        [AbpAllowAnonymous]
        public async Task PlanUserMaternityLeave(UpdateUserWorkingStatusFromHRMV2Dto input)
        {
            CheckSecurityCode();
            bool IsPool = true;
            await PlanUserFromHRMToProject(input, AppConsts.NGHI_SINH_PROJECT_CODE, IsPool);
        }

        public async Task PlanUserFromHRMToProject(UpdateUserWorkingStatusFromHRMV2Dto input, string projectCode, bool IsPool)
        {
            var activeReportId = await _resourceManager.GetActiveReportId();

            var existPlanPU = await CheckExistPlanJoinPU(input.EmailAddress, projectCode).FirstOrDefaultAsync();
            if (existPlanPU != default)
            {
                existPlanPU.StartTime = input.DateAt;
                existPlanPU.PMReportId = activeReportId;
                existPlanPU.IsPool = IsPool;
                await WorkScope.UpdateAsync(existPlanPU);
            }
            else
            {
                var projectId = await GetProjectIdByCode(projectCode);
                var userId = await WorkScope.GetAll<User>()
                    .Where(x => x.EmailAddress.ToLower().Trim() == input.EmailAddress.ToLower().Trim())
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                var newPU = new ProjectUser
                {
                    StartTime = input.DateAt,
                    ProjectId = projectId,
                    Status = ProjectUserStatus.Future,
                    AllocatePercentage = 100,
                    UserId = userId,
                    PMReportId = activeReportId,
                    IsPool = IsPool,//nghi sinh - temp -> xuat hien o pool
                };
                await WorkScope.InsertAsync(newPU);
            }
        }
        private async Task<long> GetProjectIdByCode(string code)
        {
            return await WorkScope.GetAll<Project>()
                .Where(s => s.Code == code)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }

        public IQueryable<ProjectUser> CheckExistPlanJoinPU(string email, string projectCode)
        {
            return WorkScope.GetAll<ProjectUser>()
                .Where(x => x.Project.Code == projectCode)
                .Where(x => x.User.EmailAddress.ToLower().Trim() == email.ToLower().Trim())
                .Where(x => x.Status == ProjectUserStatus.Future)
                .Where(x => x.AllocatePercentage > 0);
        }

        
       

        //Join or Update User into Project with Code = NGHI_SINH_PROJECT_CODE
        private async Task<ProjectUser> JoinOrUpdateUserIntoMaternityLeaveProject(InputUpdateUserIntoProjectDto input)
        {
            var pu = await WorkScope.GetAll<ProjectUser>()
                .Where(s => s.Status == ProjectUserStatus.Future)
                .Where(s => s.AllocatePercentage > 0)
                .Where(s => s.UserId == input.UserId)
                .Where(s => s.Project.Code == AppConsts.NGHI_SINH_PROJECT_CODE)
                .FirstOrDefaultAsync();
            if (pu != default)
            {
                pu.Status = ProjectUserStatus.Present;
                pu.StartTime = input.StartTime;
                pu.IsPool = input.IsPool;
                pu.PMReportId = input.ActiveReportId;
                await WorkScope.UpdateAsync(pu);
            }
            else
            {
                var projectId = await GetProjectIdByCode(AppConsts.NGHI_SINH_PROJECT_CODE);

                pu = new ProjectUser
                {
                    UserId = input.UserId,
                    ProjectId = projectId,
                    StartTime = input.StartTime,
                    Status = ProjectUserStatus.Present,
                    AllocatePercentage = 100,
                    IsPool = input.IsPool,
                    PMReportId = input.ActiveReportId,
                    Note = input.Note
                };
                await WorkScope.InsertAsync(pu);
            }
            return pu;
        }

        //Join or Update User into Project with Code = CHO_NGHI_PROJECT_CODE
        private async Task<ProjectUser> JoinOrUpdateUserIntoQuitProject(InputUpdateUserIntoProjectDto input)
        {
            var pu = WorkScope.GetAll<ProjectUser>()
             .Where(x => x.UserId == input.UserId)
             .Where(x => x.Status == ProjectUserStatus.Future)
             .Where(x => x.Project.Code == AppConsts.CHO_NGHI_PROJECT_CODE)
             .FirstOrDefault();
            if (pu != default)
            {
                pu.Status = ProjectUserStatus.Present;
                pu.StartTime = input.StartTime;
                pu.IsPool = input.IsPool;
                pu.PMReportId = input.ActiveReportId;
                await WorkScope.UpdateAsync(pu);
            }
            else
            {
                var projectId = await GetProjectIdByCode(AppConsts.CHO_NGHI_PROJECT_CODE);

                var newPU = new ProjectUser
                {
                    IsPool = false, // IsPool == true (Temp) IsPool == false (Official)
                    UserId = input.UserId,
                    ProjectId = projectId,
                    Status = ProjectUserStatus.Present,
                    AllocatePercentage = 100,
                    StartTime = input.StartTime,
                    PMReportId = input.ActiveReportId
                };
                await WorkScope.InsertAsync(newPU);
            }
            return pu;
        }


        private async Task<ProjectUser> JoinOrUpdateUserIntoPauseProject(InputUpdateUserIntoProjectDto input)
        {
            var pu = WorkScope.GetAll<ProjectUser>()
             .Where(x => x.UserId == input.UserId)
             .Where(x => x.Status == ProjectUserStatus.Future)
             .Where(x => x.Project.Code == AppConsts.TAM_NGHI_PROJECT_CODE)
             .FirstOrDefault();
            if (pu != default)
            {
                pu.Status = ProjectUserStatus.Present;
                pu.StartTime = input.StartTime;
                pu.IsPool = input.IsPool;
                pu.PMReportId = input.ActiveReportId;
                await WorkScope.UpdateAsync(pu);
            }
            else
            {
                var projectId = await GetProjectIdByCode(AppConsts.TAM_NGHI_PROJECT_CODE);

                var newPU = new ProjectUser
                {
                    IsPool = false, // IsPool == true (Temp) IsPool == false (Official)
                    UserId = input.UserId,
                    ProjectId = projectId,
                    Status = ProjectUserStatus.Present,
                    AllocatePercentage = 100,
                    StartTime = input.StartTime,
                    PMReportId = input.ActiveReportId
                };
                await WorkScope.InsertAsync(newPU);
            }
            return pu;
        }

        private async Task SendKomu(string komuMessage)
        {
            _komuService.NotifyToChannel(new KomuMessage
            {
                CreateDate = DateTimeUtils.GetNow(),
                Message = komuMessage,
            },
           ChannelTypeConstant.PM_CHANNEL);
        }

        private async Task SendKomu(StringBuilder sb)
        {
            await SendKomu(sb.ToString());
        }
    }

    
}
