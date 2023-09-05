using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IdentityFramework;
using Abp.Linq.Extensions;
using ProjectManagement.Authorization;
using ProjectManagement.Authorization.Roles;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Roles.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static ProjectManagement.Authorization.GrantPermissionRoles;
using NccCore.IoC;
using Abp.Authorization.Users;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManagement.Roles
{
    [AbpAuthorize(PermissionNames.Admin_Roles)]
    public class RoleAppService : AsyncCrudAppService<Role, RoleDto, int, PagedRoleResultRequestDto, CreateRoleDto, RoleDto>, IRoleAppService
    {
        private readonly RoleManager _roleManager;
        private readonly UserManager _userManager;
        private readonly IWorkScope _workScope;
        private readonly IRepository<UserRole, long> _userRoleRepository;

        public RoleAppService(IRepository<Role> repository, 
            RoleManager roleManager, 
            UserManager userManager, 
            IWorkScope workScope,
            IRepository<UserRole, long> userRoleRepository
        ) : base(repository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _workScope = workScope;
            _userRoleRepository = userRoleRepository;
        }

        [AbpAuthorize(PermissionNames.Admin_Roles_Create)]
        public override async Task<RoleDto> CreateAsync(CreateRoleDto input)
        {     

            CheckCreatePermission();

            var role = ObjectMapper.Map<Role>(input);
            role.SetNormalizedName();

            CheckErrors(await _roleManager.CreateAsync(role));

            return MapToEntityDto(role);

        }

        public async Task<ListResultDto<RoleListDto>> GetRolesAsync(GetRolesInput input)
        {
            var roles = await _roleManager
                .Roles
                .WhereIf(
                    !input.Permission.IsNullOrWhiteSpace(),
                    r => r.Permissions.Any(rp => rp.Name == input.Permission && rp.IsGranted)
                )
                .ToListAsync();

            return new ListResultDto<RoleListDto>(ObjectMapper.Map<List<RoleListDto>>(roles));
        }

        [AbpAuthorize(PermissionNames.Admin_Roles_Edit)]
        public override async Task<RoleDto> UpdateAsync(RoleDto input)
        {
            CheckUpdatePermission();
            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            ObjectMapper.Map(input, role);

            CheckErrors(await _roleManager.UpdateAsync(role));

            return input;
        }

        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var role = await _roleManager.FindByIdAsync(input.Id.ToString());
            var users = await _userManager.GetUsersInRoleAsync(role.NormalizedName);

            foreach (var user in users)
            {
                CheckErrors(await _userManager.RemoveFromRoleAsync(user, role.NormalizedName));
            }

            CheckErrors(await _roleManager.DeleteAsync(role));
        }

        public Task<ListResultDto<PermissionDto>> GetAllPermissions()
        {
            var permissions = PermissionManager.GetAllPermissions();

            return Task.FromResult(new ListResultDto<PermissionDto>(
                ObjectMapper.Map<List<PermissionDto>>(permissions).OrderBy(p => p.DisplayName).ToList()
            ));
        }

        protected override IQueryable<Role> CreateFilteredQuery(PagedRoleResultRequestDto input)
        {
            return Repository.GetAllIncluding(x => x.Permissions)
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword)
                || x.DisplayName.Contains(input.Keyword)
                || x.Description.Contains(input.Keyword));
        }

        protected override async Task<Role> GetEntityByIdAsync(int id)
        {
            return await Repository.GetAllIncluding(x => x.Permissions).FirstOrDefaultAsync(x => x.Id == id);
        }

        protected override IQueryable<Role> ApplySorting(IQueryable<Role> query, PagedRoleResultRequestDto input)
        {
            return query.OrderBy(r => r.DisplayName);
        }

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }

        public async Task<GetRoleForEditOutput> GetRoleForEdit(EntityDto input)
        {
            var permissions = SystemPermission.TreePermissions;
            var role = await _roleManager.GetRoleByIdAsync(input.Id);
            var grantedPermissions = (await _roleManager.GetGrantedPermissionsAsync(role)).ToArray();
            var users = (await _userManager.GetUsersInRoleAsync(role.NormalizedName)).ToArray();
            var roleEditDto = ObjectMapper.Map<RoleEditDto>(role);

            return new GetRoleForEditOutput
            {
                Role = roleEditDto,
                Permissions = permissions,
                GrantedPermissionNames = grantedPermissions.Select(p => p.Name).ToList(),
            };
        }

        public async Task<RolePermissionDto> ChangeRolePermission(RolePermissionDto input)
        {
            CheckUpdatePermission();

            var role = await _roleManager.GetRoleByIdAsync(input.Id);

            var p = PermissionManager.GetAllPermissions().ToList();
            var grantedPermissions = PermissionManager
               .GetAllPermissions()
               .Where(p => input.Permissions.Contains(p.Name))
               .ToList();

            await _roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);

            return input;
        }

        public async Task<List<UserRoleDto>> GetAllUserInRole(long Id)
        {
            var qallUsers = from ur in _userRoleRepository.GetAll()
                            join u in _workScope.GetAll<User>() on ur.UserId equals u.Id
                            where ur.RoleId == Id
                            select new UserRoleDto
                            {
                                Id = ur.Id,
                                UserId = u.Id,
                                FullName = u.FullName,
                                AvatarPath = u.AvatarPath,
                                Branch = u.BranchOld,
                                Email = u.EmailAddress,
                                UserLevel = u.UserLevel,
                                UserName = u.UserName,
                                UserType = u.UserType,
                                PositionId = u.PositionId,
                                PositionName = u.Position.ShortName,
                                PositionColor = u.Position.Color
                            };
            var allUsers = await qallUsers.Distinct().ToListAsync();
            return allUsers;
        }

        [HttpGet]
        public async Task<string> RemoveUserFromOutRole(long Id)
        {
            await _userRoleRepository.DeleteAsync(Id);
            return "Deleted successfully";
        }

        [HttpPost]
        public async Task<string> AddUserIntoRole(CreateUserRoleDto input)
        {
            await _userRoleRepository.InsertAsync(new UserRole
            {
                RoleId = input.RoleId,
                UserId = input.UserId,
            });
            return "Added successfully";
        }

        public IActionResult GetAllUserLevel()
        {
            var allUserLevels = Enum.GetValues(typeof(UserLevel))
                                    .Cast<UserLevel>()
                                    .Select(x => new
                                    {
                                        Id = ((int)x),
                                        Name = x.ToString()
                                    })
                                    .ToList();
            return new OkObjectResult(allUserLevels);
        }

        public async Task<IActionResult> GetAllUserNotInRole(int RoleId)
        {
            var listUsersInRole = await _userRoleRepository.GetAll()
                .Where(x => x.RoleId == RoleId)
                .Select(x => x.UserId)
                .Distinct()
                .ToListAsync();
            var qallUsersNotInRole = from u in _workScope.GetAll<User>()
                            where !listUsersInRole.Contains(u.Id) && 
                                  (u.UserType != UserType.FakeUser || u.UserType != UserType.Vendor)
                            select new
                            {
                                Id = u.Id,
                                Surname = u.Surname,
                                Name = u.Name,
                                FullName = u.FullName,
                                Email = u.EmailAddress,
                            };
            var allUserNotInRole = await qallUsersNotInRole.Distinct().ToListAsync();
            return new OkObjectResult(allUserNotInRole);
        }
    }
}

