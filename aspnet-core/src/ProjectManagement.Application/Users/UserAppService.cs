using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.UI;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Accounts;
using ProjectManagement.Authorization.Roles;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Roles.Dto;
using ProjectManagement.Users.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using OfficeOpenXml;
using NccCore.IoC;
using Abp.Authorization.Users;
using Microsoft.AspNetCore.Hosting;
using ProjectManagement.Entities;
using NccCore.Paging;
using NccCore.Extension;
using ProjectManagement.Services.HRM;
using ProjectManagement.Services.HRM.Dto;
using Abp.Configuration;
using ProjectManagement.Services.Komu;
using ProjectManagement.Services.Komu.KomuDto;
using System.Text;
using ProjectManagement.Constants;
using NccCore.Uitls;
using ProjectManagement.NccCore.Helper;
using Microsoft.AspNetCore.Http;
using ProjectManagement.Configuration;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Constants.Enum;
using NccCore.Helper;
using ProjectManagement.Services.ResourceService.Dto;
using ProjectManagement.Services.ResourceRequestService.Dto;
using ProjectManagement.Utils;
using ProjectManagement.UploadFilesService;

namespace ProjectManagement.Users
{
    [AbpAuthorize]
    public class UserAppService : AsyncCrudAppService<User, UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>, IUserAppService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAbpSession _abpSession;
        private readonly LogInManager _logInManager;
        private readonly IWorkScope _workScope;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HRMService _hrmService;
        private ISettingManager _settingManager;
        private KomuService _komuService;
        private UploadFileService _uploadFileService;
        public UserAppService(
            IRepository<User, long> repository,
            UserManager userManager,
            RoleManager roleManager,
            IRepository<Role> roleRepository,
            IPasswordHasher<User> passwordHasher,
            IAbpSession abpSession,
            LogInManager logInManager,
            IWorkScope workScope,
            IWebHostEnvironment webHostEnvironment,
            HRMService hrmService,
            KomuService komuService,
            ISettingManager settingManager,
            IHttpContextAccessor httpContextAccessor,
            UploadFileService uploadFileService
         )
            : base(repository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _passwordHasher = passwordHasher;
            _abpSession = abpSession;
            _logInManager = logInManager;
            _workScope = workScope;
            _webHostEnvironment = webHostEnvironment;
            _hrmService = hrmService;
            _komuService = komuService;
            _settingManager = settingManager;
            _httpContextAccessor = httpContextAccessor;
            _uploadFileService = uploadFileService;
        }


        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Users)]
        public async Task<GridResult<GetAllUserDto>> GetAllPaging(InputGetAllUserDto input)
        {
            var quser = from u in _workScope.All<User>()
                        select new GetAllUserDto
                        {

                            Id = u.Id,
                            EmailAddress = u.EmailAddress,
                            AvatarPath = u.AvatarPath == null ? "" : u.AvatarPath,
                            UserType = u.UserType,
                            PositionId = u.PositionId,
                            PositionColor = u.Position.Color,
                            PositionName = u.Position.ShortName,
                            UserLevel = u.UserLevel,
                            Branch = u.BranchOld,
                            BranchColor = u.Branch.Color,
                            BranchDisplayName = u.Branch.DisplayName,
                            IsActive = u.IsActive,
                            FullName = u.Name + " " + u.Surname,
                            CreationTime = u.CreationTime,
                            RoleNames = _roleManager.Roles
                            .Where(r => u.Roles
                            .Select(x => x.RoleId)
                            .Contains(r.Id))
                            .Select(r => r.NormalizedName).ToArray(),
                            UserSkills = u.UserSkills.Select(s => new UserSkillDto
                            {
                                UserId = u.Id,
                                SkillId = s.SkillId,
                                SkillName = s.Skill.Name
                            }).ToList(),
                            WorkingProjects = u.ProjectUsers
                            .Where(s => s.Status == ProjectUserStatus.Present && s.AllocatePercentage > 0)
                            .Where(x => x.Project.Status != ProjectStatus.Potential && x.Project.Status != ProjectStatus.Closed)
                            .Select(p => new WorkingProjectDto
                            {
                                ProjectName = p.Project.Name,
                                ProjectRole = p.ProjectRole,
                                StartTime = p.StartTime,
                                IsPool = p.IsPool

                            }).ToList(),
                        };

            if (input.SkillIds != null && !input.SkillIds.IsEmpty())
            {
                var qSkillUserIds = _workScope.GetAll<UserSkill>()
                    .Where(s => input.SkillIds.Contains(s.SkillId))
                    .Select(s => s.UserId);

                quser = from u in quser
                        join userId in qSkillUserIds on u.Id equals userId
                        select u;
            }

            return await quser.GetGridResult(quser, input);

        }

        [HttpGet]
        [AbpAuthorize]
        public async Task<List<ProjectHistoryDto>> GetHistoryProjectsByUserId(long userId)
        {
            return await _workScope.GetAll<ProjectUser>().Where(s => s.UserId == userId)
                .Where(sa => sa.Status == ProjectUserStatus.Past || (sa.Status == ProjectUserStatus.Present & sa.AllocatePercentage <= 0))
                .Select(pu => new ProjectHistoryDto
                {
                    ProjectId = pu.ProjectId,
                    ProjectName = pu.Project.Name,
                    ProjectRole = pu.ProjectRole,
                    allowcatePercentage = pu.AllocatePercentage,
                    StartTime = pu.StartTime,
                    Status = pu.Status,
                })
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize]

        public async Task updateUserSkill(UpdateUserSkillDto input)
        {
            var user = await _workScope.GetAsync<User>(input.UserId);
            if (user == default)
                throw new UserFriendlyException($"Can not found user with Id = {input.UserId}");
            // check exception
            if (input.UserSkills.Any(i => i.SkillRank < SkillRank.None || i.SkillRank > SkillRank.Expert))
                throw new UserFriendlyException("Skill rank must be from 1 to 5!");
            var userSkills = await _workScope.GetAll<UserSkill>().Where(x => x.UserId == input.UserId).ToListAsync();
            var currenUserSkillId = userSkills.Select(x => x.SkillId);

            var deleteSkillId = currenUserSkillId.Except(input.UserSkills.Select(u => u.SkillId));
            var listSkillDelete = userSkills.Where(x => deleteSkillId.Contains(x.SkillId));
            var listSkillInsert = input.UserSkills.Where(x => !currenUserSkillId.Contains(x.SkillId));
            var listSkillUpdate = input.UserSkills.Where(x => !listSkillInsert.Contains(x));

            foreach (var item in listSkillDelete)
            {
                await _workScope.DeleteAsync<UserSkill>(item);
            }

            var userSkillInserts = listSkillInsert.Select(x => new UserSkill
            { UserId = input.UserId, SkillId = x.SkillId, SkillRank = x.SkillRank });
            await _workScope.InsertRangeAsync(userSkillInserts);

            var userSkillUpdates = new List<UserSkill>();
            foreach (var item in listSkillUpdate)
            {
                var userSkill = _workScope.GetAll<UserSkill>()
                .Where(u => u.UserId == input.UserId && u.SkillId == item.SkillId).FirstOrDefault();
                userSkill.SkillRank = item.SkillRank;
                userSkillUpdates.Add(userSkill);
            }
            await _workScope.UpdateRangeAsync(userSkillUpdates);
        }


        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Users_ActiveAndDeactive)]
        public async Task updateUserActive(long userId, bool isActive)
        {
            await _userManager.UpdateUserActive(userId, isActive);
        }

        [HttpDelete]
        [AbpAuthorize(PermissionNames.Admin_Users_DeleteFakeUser)]
        public async Task DeleteFakeUser(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user.UserType != UserType.FakeUser)
            {
                throw new UserFriendlyException(String.Format("You can delete FakeUser only!"));
            }
            await _userManager.DeleteAsync(user);
        }


        [AbpAuthorize(PermissionNames.Admin_Users_Create)]
        public override async Task<UserDto> CreateAsync(CreateUserDto input)
        {
            CheckCreatePermission();

            var user = ObjectMapper.Map<User>(input);

            user.TenantId = AbpSession.TenantId;
            user.IsEmailConfirmed = true;

            await _userManager.InitializeOptionsAsync(AbpSession.TenantId);

            CheckErrors(await _userManager.CreateAsync(user, input.Password));

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            if (input.UserSkills != null)
            {
                foreach (var s in input.UserSkills)
                {
                    var skill = new UserSkill
                    {
                        UserId = user.Id,
                        SkillId = s.SkillId
                    };
                    await _workScope.InsertAndGetIdAsync(skill);
                }
            }

            CurrentUnitOfWork.SaveChanges();

            return MapToEntityDto(user);
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Users_Edit)]
        public async Task<IActionResult> UpdateUserInfo(CreateUpdateUserDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.Id);

            ObjectMapper.Map(input, user);
            CheckErrors(await _userManager.UpdateAsync(user));

            return new OkObjectResult("Update succesful");
        }

        [HttpPut]
        [AbpAuthorize(PermissionNames.Admin_Users_UpdateRole)]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleDto input)
        {
            var user = await _userManager.GetUserByIdAsync(input.UserId);

            if (input.RoleNames != null)
            {
                CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
            }

            return new OkObjectResult("Update succesful");
        }




        [AbpAuthorize(PermissionNames.Admin_Users_Edit)]
        public override async Task<UserDto> UpdateAsync(UserDto input)
        {
            bool isUpdateAll = await PermissionChecker.IsGrantedAsync(PermissionNames.Admin_Users_Edit);

            CheckUpdatePermission();

            if (isUpdateAll)
            {

                var user = await _userManager.GetUserByIdAsync(input.Id);

                MapToEntity(input, user);

                CheckErrors(await _userManager.UpdateAsync(user));

                if (input.RoleNames != null)
                {
                    CheckErrors(await _userManager.SetRolesAsync(user, input.RoleNames));
                }
            }

            var userSkills = await _workScope.GetAll<UserSkill>().Where(x => x.UserId == input.Id).ToListAsync();
            var currenUserSkillId = userSkills.Select(x => x.SkillId);

            var deleteSkillId = currenUserSkillId.Except(input.UserSkills.Select(x => x.SkillId));
            var deleteSkill = userSkills.Where(x => deleteSkillId.Contains(x.SkillId));
            var addSkill = input.UserSkills.Where(x => !currenUserSkillId.Contains(x.SkillId));

            foreach (var item in deleteSkill)
            {
                await _workScope.DeleteAsync<UserSkill>(item);
            }

            foreach (var item in addSkill)
            {
                var userSkill = new UserSkill
                {
                    UserId = item.UserId,
                    SkillId = item.SkillId
                };
                await _workScope.InsertAndGetIdAsync(userSkill);
            }

            return await GetAsync(input);
        }

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<EmployeeInformationDto> GetEmployeeInformation(string email)
        {
            //if (!CheckSecurityCode())
            //{
            //    throw new UserFriendlyException("SecretCode does not match!");
            //}
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var userAndBranch = await _workScope
                .GetAll<User>()
                .Where(s => s.EmailAddress == email)
                .Select(s => new { User = s, BranchName = s.Branch != null ? s.Branch.Name : "" })
                .FirstOrDefaultAsync();

            if (userAndBranch == null)
            {
                return null;
            }
            var user = userAndBranch.User;

            if (string.IsNullOrEmpty(user.PhoneNumber) || !user.DOB.HasValue || !user.Job.HasValue)
            {
                await UpdateInfomationFromHRM(user);
            }

            var qProjectUsers = from pu in _workScope
                                .GetAll<ProjectUser>()
                                .Where(x => x.UserId == user.Id &&
                                            x.Project.Status != ProjectStatus.Closed &&
                                            x.Status == ProjectUserStatus.Present &&
                                            x.AllocatePercentage > 50)
                                select new
                                {
                                    ProjectId = pu.ProjectId,
                                    ProjectName = pu.Project.Name,
                                    PmName = pu.Project.PM.FullName,
                                    StartTime = pu.StartTime,
                                    ProjectRole = pu.ProjectRole,
                                    PmEmail = pu.Project.PM.EmailAddress
                                };
            var projectUsers = await qProjectUsers.OrderByDescending(x => x.StartTime).ToListAsync();

            var employeeInfo = new EmployeeInformationDto()
            {
                EmployeeId = user.Id,
                EmailAddress = user.EmailAddress,
                EmployeeName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                DOB = user.DOB,
                Branch = userAndBranch.BranchName,
                UserType = user.UserType,
                Job = user.Job,
                ProjectDtos = projectUsers.Select(x =>
                        new ProjectDTO()
                        {
                            ProjectId = x.ProjectId,
                            ProjectName = x.ProjectName,
                            PmName = x.PmName,
                            StartTime = x.StartTime,
                            PRole = x.ProjectRole,
                            PmEmail = x.PmEmail
                        }).ToList()
            };



            return employeeInfo;
        }

        [AbpAuthorize(PermissionNames.Admin_Users_DeleteFakeUser)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            var hasProject = await _workScope.GetAll<Project>().AnyAsync(x => x.PMId == input.Id);
            if (hasProject)
                throw new UserFriendlyException("User is a project manager !");

            var useSkills = await _workScope.GetAll<UserSkill>().Where(x => x.UserId == input.Id).ToListAsync();
            foreach (var item in useSkills)
            {
                await _workScope.DeleteAsync(item);
            }

            var user = await _userManager.GetUserByIdAsync(input.Id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<ListResultDto<RoleDto>> GetRoles()
        {
            var roles = await _roleRepository.GetAllListAsync();
            return new ListResultDto<RoleDto>(ObjectMapper.Map<List<RoleDto>>(roles));
        }

        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                AbpSession.ToUserIdentifier(),
                LocalizationSettingNames.DefaultLanguage,
                input.LanguageName
            );
        }

        protected override User MapToEntity(CreateUserDto createInput)
        {
            var user = ObjectMapper.Map<User>(createInput);
            user.SetNormalizedNames();
            return user;
        }

        protected override void MapToEntity(UserDto input, User user)
        {
            ObjectMapper.Map(input, user);
            user.SetNormalizedNames();
        }

        protected override UserDto MapToEntityDto(User user)
        {
            var roleIds = user.Roles.Select(x => x.RoleId).ToArray();

            var roles = _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).Select(r => r.NormalizedName);
            var userSkill = _workScope.GetAll<UserSkill>().Where(x => x.UserId == user.Id).Select(x => new UserSkillDto
            {
                UserId = x.UserId,
                SkillId = x.SkillId,
                SkillName = x.Skill.Name
            });
            var userDto = base.MapToEntityDto(user);
            userDto.RoleNames = roles.ToArray();
            userDto.UserSkills = userSkill.ToList();
            userDto.PositionId = user.PositionId;

            return userDto;
        }

        protected override IQueryable<User> CreateFilteredQuery(PagedUserResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Roles)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.UserName.Contains(input.Keyword) || x.Name.Contains(input.Keyword) || x.EmailAddress.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }

        protected override async Task<User> GetEntityByIdAsync(long id)
        {
            var user = await Repository.GetAllIncluding(x => x.Roles).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException(typeof(User), id);
            }

            return user;
        }

        protected override IQueryable<User> ApplySorting(IQueryable<User> query, PagedUserResultRequestDto input)
        {
            return query.OrderBy(r => r.UserName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
        public async Task<bool> ChangePassword(ChangePasswordDto input)
        {
            if (_abpSession.UserId == null)
            {
                throw new UserFriendlyException("Please log in before attemping to change password.");
            }
            long userId = _abpSession.UserId.Value;
            var user = await _userManager.GetUserByIdAsync(userId);
            var loginAsync = await _logInManager.LoginAsync(user.UserName, input.CurrentPassword, shouldLockout: false);
            if (loginAsync.Result != AbpLoginResultType.Success)
            {
                throw new UserFriendlyException("Your 'Existing Password' did not match the one on record.  Please try again or contact an administrator for assistance in resetting your password.");
            }
            if (!new Regex(AccountAppService.PasswordRegex).IsMatch(input.NewPassword))
            {
                throw new UserFriendlyException("Passwords must be at least 8 characters, contain a lowercase, uppercase, and number.");
            }
            user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
            CurrentUnitOfWork.SaveChanges();
            return true;
        }

        //[AbpAllowAnonymous]
        //public async Task<List<string>> ResetPasswordAllAccount()
        //{
        //    var users = await Repository.GetAll().Where(s => s.IsActive).ToListAsync();
        //    foreach (var user in users)
        //    {
        //        user.Password = _passwordHasher.HashPassword(user, "123qwe");
        //    }
        //    CurrentUnitOfWork.SaveChanges();
        //    return users.Select(s => s.UserName).ToList();

        //}

        [AbpAuthorize(PermissionNames.Admin_Users_ResetPassword)]
        public async Task<bool> ResetPassword(ResetPasswordDto input)
        {

            //long currentUserId = _abpSession.UserId.Value;
            //var currentUser = await _userManager.GetUserByIdAsync(currentUserId);
            //var loginAsync = await _logInManager.LoginAsync(currentUser.UserName, input.AdminPassword, shouldLockout: false);
            //if (loginAsync.Result != AbpLoginResultType.Success)
            //{
            //    throw new UserFriendlyException("Your 'Admin Password' did not match the one on record.  Please try again.");
            //}
            //if (currentUser.IsDeleted || !currentUser.IsActive)
            //{
            //    return false;
            //}
            //var roles = await _userManager.GetRolesAsync(currentUser);
            //if (!roles.Contains(StaticRoleNames.Tenants.Admin))
            //{
            //    throw new UserFriendlyException("Only administrators may reset passwords.");
            //}

            var user = await _userManager.GetUserByIdAsync(input.UserId);
            if (user != null)
            {
                user.Password = _passwordHasher.HashPassword(user, input.NewPassword);
                CurrentUnitOfWork.SaveChanges();
            }

            return true;
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAllUserActive(bool onlyStaff)
        {
            var query = _workScope.GetAll<User>()
                .Where(u => u.IsActive)
                .Where(x => x.UserType != UserType.Vendor)
                .Where(x => x.UserType != UserType.FakeUser)
                .Where(x => onlyStaff ? x.UserType != UserType.Internship : true)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Name = u.Name,
                    Surname = u.Surname,
                    EmailAddress = u.EmailAddress,
                    FullName = u.FullName,
                    AvatarPath = u.AvatarPath,
                    UserType = u.UserType,
                    UserLevel = u.UserLevel,
                    Branch = u.BranchOld,
                    PositionId = u.PositionId,
                    UserSkills = u.UserSkills.Select(x => new UserSkillDto
                    {
                        SkillId = x.SkillId,
                        SkillName = x.Skill.Name
                    }).ToList()
                });
            return await query.ToListAsync();
        }

        [HttpGet]
        public async Task<List<UserDto>> GetAllWithDeactiveUser()
        {
            var query = _workScope.GetAll<User>()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Name = u.Name,
                    Surname = u.Surname,
                    EmailAddress = u.EmailAddress,
                    FullName = u.FullName,
                    AvatarPath = u.AvatarPath,
                    UserType = u.UserType,
                    UserLevel = u.UserLevel,
                    Branch = u.BranchOld,
                    PositionId = u.PositionId,
                    IsActive = u.IsActive,
                    UserCode = u.UserCode
                });
            return await query.ToListAsync();
        }

        [HttpPost]
        [AbpAuthorize(PermissionNames.Admin_Users_UploadAvatar)]
        public async Task<string> UpdateAvatar([FromForm] AvatarDto input)
        {
            User user = await _userManager.GetUserByIdAsync(input.UserId);
            String avatarPath = await _uploadFileService.UploadAvatarAsync(input.File);
            user.AvatarPath = avatarPath;
            await _userManager.UpdateAsync(user);
            return avatarPath;

        }

        [HttpPost]
        public async Task<Object> ImportUserFromFile([FromForm] FileInputDto input)
        {
            if (input == null)
            {
                throw new UserFriendlyException(String.Format("No file upload!"));
            }

            var path = new String[] { ".xlsx", ".xltx" };
            if (!path.Contains(Path.GetExtension(input.File.FileName)))
            {
                throw new UserFriendlyException(String.Format("Invalid file upload, only acept extension .xlsx, .xltx"));
            }

            List<User> listUser = new List<User>();
            var failedList = new List<string>();
            var successList = new List<User>();

            var roleEmployee = _roleManager.GetRoleByName(RoleConstants.ROLE_EMPLOYEE);

            using (var stream = new MemoryStream())
            {
                await input.File.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var fullName = worksheet.Cells[row, 3].Value.ToString().Trim();
                        var name = SplitUsername(fullName);

                        listUser.Add(new User
                        {
                            UserName = $"{name.Name.Trim().ToLower()}.{name.SurName.Replace(" ", "").ToLower()}",
                            Name = name.Name,
                            Surname = name.SurName,
                            EmailAddress = worksheet.Cells[row, 8].Value.ToString().Trim().ToLower(),
                            NormalizedEmailAddress = worksheet.Cells[row, 8].Value.ToString().Trim().ToUpper(),
                            NormalizedUserName = "DEFAULT",
                            IsActive = true,
                            Password = "123qwe",
                            UserCode = worksheet.Cells[row, 2].Value.ToString().Trim()
                        });
                    }
                }
            }

            int index = 1;
            foreach (var user in listUser)
            {
                try
                {
                    user.Id = await _workScope.InsertAndGetIdAsync(user);

                    var userRole = new UserRole
                    {
                        UserId = user.Id,
                        RoleId = roleEmployee.Id
                    };
                    await _workScope.InsertAndGetIdAsync(userRole);

                    successList.Add(user);
                }
                catch (Exception e)
                {
                    failedList.Add("Error: Row: " + index + ". " + e.Message);
                }
                index++;
            }

            if (successList.Count() < 1)
            {
                throw new UserFriendlyException(String.Format("Invalid excel data."));
            }
            return new { successList, failedList };
        }
        [AbpAuthorize(PermissionNames.Admin_Users_SyncDataFromHrm)]
        public async Task<object> AutoUpdateUserFromHRM()
        {
            var hrmUsers = await _hrmService.GetUserFromHRM();

            var hrmEmails = hrmUsers.Select(x => x.EmailAddress.ToLower().Trim())
                .ToHashSet();

            var projectUsers = await _workScope.GetAll<User>()
                .ToListAsync();

            var projectEmails = projectUsers.Select(x => x.EmailAddress.ToLower().Trim())
                .ToHashSet();

            var mapProjectUser = projectUsers.GroupBy(s => s.EmailAddress.ToLower().Trim())
                .ToDictionary(s => s.Key, s => s.FirstOrDefault());

            var insertSuccessful = new List<string>();
            var insertFailed = new List<string>();
            var updateSuccessful = new List<string>();
            var updateFakeUser = new List<string>();
            var updateFailed = new List<string>();
            var komuMessage = new StringBuilder();
            komuMessage.Append("Welcome các nhân viên mới vào làm việc tại công ty, đó là ");

            var fakeProjectUsers = projectUsers
                .Where(x => !hrmEmails.Contains(x.EmailAddress))
                .ToList();

            foreach (var fakeUser in fakeProjectUsers)
            {
                if (fakeUser.UserType == UserType.FakeUser || !fakeUser.IsActive)
                {
                    continue;
                }
                try
                {
                    fakeUser.UserType = UserType.FakeUser;
                    await _workScope.UpdateAsync(fakeUser);
                    updateFakeUser.Add(fakeUser.EmailAddress);
                }
                catch (Exception e)
                {
                    updateFailed.Add(fakeUser.EmailAddress + " error =>" + e.Message);
                }
            }

            foreach (var hrmUser in hrmUsers)
            {

                var pUser = mapProjectUser.ContainsKey(hrmUser.EmailAddress.ToLower().Trim()) ?
                    mapProjectUser[hrmUser.EmailAddress.ToLower().Trim()] : null;

                if (pUser == null && !hrmUser.IsActive)
                {
                    continue;
                }

                if (pUser != null)
                {
                    try
                    {
                        if (pUser.IsActive && hrmUser.Status == Status.Quit)
                        {
                            await ProcessUserQuitJob(pUser, hrmUser.StartDayOff.HasValue ? hrmUser.StartDayOff.Value : DateTimeUtils.GetNow());
                            continue;
                        }

                        if (IsTheSame(hrmUser, pUser))
                        {
                            continue;
                        }
                        ObjectMapper.Map(hrmUser, pUser);
                        pUser.UserName = hrmUser.EmailAddress.ToLower().Trim();
                        pUser.UserCode = hrmUser.UserCode;
                        await _workScope.UpdateAsync(pUser);

                        updateSuccessful.Add(hrmUser.EmailAddress);
                    }
                    catch (Exception ex)
                    {
                        updateFailed.Add($"Update Failed: {hrmUser.EmailAddress} => Error(): {ex.Message}");
                    }
                    continue;
                }

                try
                {
                    var newUser = await InsertUserFromHRM(hrmUser);
                    insertSuccessful.Add(hrmUser.EmailAddress);

                    hrmUser.UserName = UserHelper.GetUserName(hrmUser.EmailAddress) ?? hrmUser.UserName;
                    var userKomuId = await UpdateKomuId(newUser.Id);
                    komuMessage.Append($"{(userKomuId.HasValue ? "<@" + userKomuId + ">" : "**" + hrmUser.UserName + "**")}, ");
                }
                catch (Exception ex)
                {
                    insertFailed.Add($"Insert Failed: {hrmUser.EmailAddress} => Error(): {ex.Message}");
                }
            }
            if (insertSuccessful.Count > 0)
            {
                komuMessage = komuMessage.Length >= 2 ? komuMessage.Remove(komuMessage.Length - 2, 2) : komuMessage;
                komuMessage.Append("\rCác PM hãy nhanh tay pick nhân viên vào dự án ngay nào.");
                _komuService.NotifyToChannel(new KomuMessage
                {
                    Message = komuMessage.ToString(),
                    CreateDate = DateTimeUtils.GetNow(),
                },
               ChannelTypeConstant.GENERAL_CHANNEL);
            }
            return new
            {
                insertSuccessful,
                insertFailed,
                updateSuccessful,
                updateFailed,
                updateFakeUser
            };
        }


        public async Task<List<UserBaseDto>> GetAllActiveUser()
        {
            return await _workScope.GetAll<User>()
                .Where(s => s.IsActive)
                .Where(s => s.UserType != UserType.FakeUser)
                .Select(s => new UserBaseDto
                {
                    AvatarPath = s.AvatarPath,
                    Branch = s.BranchOld,
                    PositionName = s.Position.ShortName,
                    EmailAddress = s.EmailAddress,
                    FullName = s.FullName,
                    Id = s.Id,
                    UserLevel = s.UserLevel,
                    UserType = s.UserType,

                })
                .ToListAsync();
        }


        [HttpPost]
        //[AbpAuthorize(PermissionNames.Pages_Users_UpdateStarRateFromTimesheet)]
        [AbpAllowAnonymous]
        public async Task<List<UpdateStarRateFromTimesheetDto>> UpdateStarRateFromTimesheet(List<UpdateStarRateFromTimesheetDto> input)
        {
            if (!CheckSecurityCode())
            {
                throw new UserFriendlyException("SecretCode does not match!");
            }
            foreach (var item in input)
            {
                var user = await _workScope.GetAll<User>().Where(x => x.EmailAddress == item.EmailAddress).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.StarRate = item.StarRate;
                    user.UserLevel = item.Level;
                    if (user.UserLevel >= UserLevel.FresherMinus && item.Type == CommonUtil.TimeSheetUserType.Staff)
                    {
                        user.UserType = UserType.ProbationaryStaff;
                    }
                    else
                    {
                        user.UserType = CommonUtil.GetProjectUserType((byte)item.Type);
                    }
                    await _workScope.UpdateAsync(user);
                }
            }
            CurrentUnitOfWork.SaveChanges();
            return input;
        }

        public async Task<long?> GetKomuIdUpdateIfEmpty(User user)
        {
            if (user.KomuUserId.HasValue)
            {
                return user.KomuUserId.Value;
            }

            var userName = UserHelper.GetUserName(user.EmailAddress);

            user.KomuUserId = await _komuService.GetKomuUserId(new KomuUserDto
            {
                Username = userName,
            });

            if (user.KomuUserId.HasValue)
            {
                await _userManager.UpdateAsync(user);
                return user.KomuUserId;
            }

            return null;
        }

        public async Task<long?> UpdateKomuId(long userId)
        {
            var user = await _userManager.GetUserByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            if (user.KomuUserId.HasValue)
            {
                return user.KomuUserId;
            }
            var userName = UserHelper.GetUserName(user.EmailAddress);
            if (userName != null)
            {
                user.KomuUserId = await _komuService.GetKomuUserId(new KomuUserDto
                {
                    Username = userName ?? user.UserName,
                });
                if (user.KomuUserId.HasValue)
                {
                    await _userManager.UpdateAsync(user);
                    return user.KomuUserId;
                }
            }
            return null;
        }
        #region PRIVATE API

        private async Task ProcessUserQuitJob(User user, DateTime offDate)
        {
            user.IsActive = false;
            await _workScope.UpdateAsync(user);
            var presentPUs = await _workScope.GetAll<ProjectUser>()
                .Where(s => s.UserId == user.Id)
                .Where(s => s.Status == ProjectUserStatus.Present)
                .Where(s => s.AllocatePercentage > 0)
                .ToListAsync();

            if (presentPUs != null && presentPUs.Count > 0)
            {
                var pmReportId = await _workScope.GetAll<PMReport>()
                    .Where(s => s.IsActive)
                    .OrderByDescending(s => s.CreationTime)
                    .Select(s => s.Id)
                    .FirstOrDefaultAsync();

                foreach (var pu in presentPUs)
                {
                    pu.Status = ProjectUserStatus.Past;
                    await _workScope.UpdateAsync(pu);
                    var newPU = new ProjectUser
                    {
                        ProjectId = pu.ProjectId,
                        UserId = pu.UserId,
                        StartTime = offDate,
                        Status = ProjectUserStatus.Present,
                        AllocatePercentage = 0,
                        ProjectRole = pu.ProjectRole,
                        Note = "Auto ra khỏi project vì nghỉ việc",
                        PMReportId = pmReportId
                    };
                    await _workScope.InsertAsync(newPU);
                }
            }

        }

        private async Task<UserDto> InsertUserFromHRM(AutoUpdateUserDto user)
        {
            var createUser = new CreateUserDto
            {
                UserName = user.EmailAddress,
                Name = user.Name,
                Surname = user.Surname,
                EmailAddress = user.EmailAddress,
                UserCode = user.UserCode,
                UserType = user.UserType,
                UserLevel = user.UserLevel,
                BranchId = user.BranchId,
                IsActive = user.IsActive,
                Password = RandomPasswordHelper.CreateRandomPassword(),
                RoleNames = new string[] { "EMPLOYEE" }
            };

            return await CreateAsync(createUser);
        }
        private NameSplitDto SplitUsername(string fullName)
        {
            var name = "";
            var surName = "";
            for (int i = 0; i < fullName.Length; i++)
            {
                if (fullName[i] == ' ')
                {
                    name = fullName.Substring(0, i);
                    surName = fullName.Substring(i + 1, fullName.Length - i - 1);
                    break;
                }
            }
            return new NameSplitDto
            {
                Name = name,
                SurName = surName
            };
        }
        private bool CheckSecurityCode()
        {
            var secretCode = SettingManager.GetSettingValue(AppSettingNames.SecurityCode);
            var header = _httpContextAccessor.HttpContext.Request.Headers;
            var securityCodeHeader = header["X-Secret-Key"];
            if (secretCode == securityCodeHeader)
                return true;
            return false;
        }
        private async Task UpdateInfomationFromHRM(User user)
        {
            var userFromHRM = await _hrmService.GetUserFromHRMByEmail(user.EmailAddress);
            if (userFromHRM == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(userFromHRM.PhoneNumber) &&
                    !userFromHRM.DOB.HasValue &&
                    !userFromHRM.Job.HasValue)
            {
                return;
            }

            user.PhoneNumber = userFromHRM.PhoneNumber;
            user.DOB = userFromHRM.DOB;
            user.Job = userFromHRM.Job;
            await _workScope.UpdateAsync(user);
        }

        private bool IsTheSame(AutoUpdateUserDto hrmUser, User projectUser)
        {
            return string.Equals(hrmUser.PhoneNumber, projectUser.PhoneNumber)
                && string.Equals(hrmUser.Name, projectUser.Name)
                && string.Equals(hrmUser.Surname, projectUser.Surname)
                && string.Equals(hrmUser.UserType, projectUser.UserType)
                && string.Equals(hrmUser.UserLevel, projectUser.UserLevel)
                && string.Equals(hrmUser.Branch, projectUser.Branch)
                && string.Equals(hrmUser.FullName, projectUser.FullName)
                && string.Equals(hrmUser.IsActive, projectUser.IsActive);

        }

        #endregion
    }
}

