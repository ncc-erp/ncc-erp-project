using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.APIs.ConfigITs.Dto;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ConfigITs
{
    [AbpAuthorize]
    public class ConfigITAppService : ProjectManagementAppServiceBase
    {
        [HttpGet]
        public async Task<List<ConfigITDto>> GetAll(string searchText = null)
        {
            var query = WorkScope.GetAll<ConfigIT>()
                .Include(x => x.User)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.User)
                    .ThenInclude(x => x.Position)
                .Select(x => new ConfigITDto
                {
                    Id = x.Id,
                    EmailAddress = x.User.EmailAddress,
                    AvatarPath = x.User.AvatarPath,
                    UserType = x.User.UserType,
                    UserLevel = x.User.UserLevel,
                    Branch = x.User.BranchOld,
                    FullName = x.User.Name + " " + x.User.Surname,
                    CreationTime = x.CreationTime,
                    BranchColor = x.User.Branch != null ? x.User.Branch.Color : null,
                    BranchDisplayName = x.User.Branch != null ? x.User.Branch.DisplayName : null,
                    PositionId = x.User.PositionId,
                    PositionColor = x.User.Position != null ? x.User.Position.Color : null,
                    PositionName = x.User.Position != null ? x.User.Position.ShortName : null,
                    UserId = x.UserId
                });

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var text = searchText.Trim();
                query = query.Where(x => x.FullName != null && x.FullName.Contains(text));
            }

            return await query
                .OrderByDescending(x => x.CreationTime)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ConfigITDto> Create(long userId)
        {
            var user = await WorkScope.GetAll<ProjectManagement.Authorization.Users.User>()
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null)
                throw new UserFriendlyException("User not found!");

            var exists = await WorkScope.GetAll<ConfigIT>().AnyAsync(x => x.UserId == userId);
            if (exists)
                throw new UserFriendlyException("Config already exists for this user!");

            var config = new ConfigIT
            {
                UserId = userId,
                TenantId = AbpSession.TenantId
            };

            var id = await WorkScope.InsertAndGetIdAsync(config);

            return await GetById(id);
        }

        [HttpPut]
        public async Task<ConfigITDto> Update(long id, long userId)
        {
            var config = await WorkScope.GetAsync<ConfigIT>(id);
            if (config == null)
                throw new UserFriendlyException("Config not found!");

            var user = await WorkScope.GetAll<ProjectManagement.Authorization.Users.User>()
                .FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
                throw new UserFriendlyException("User not found!");

            var exists = await WorkScope.GetAll<ConfigIT>()
                .AnyAsync(x => x.Id != id && x.UserId == userId);
            if (exists)
                throw new UserFriendlyException("Config already exists for this user!");

            config.UserId = userId;
            await WorkScope.UpdateAsync(config);

            return await GetById(id);
        }

        [HttpGet]
        public async Task<List<ConfigITUserDto>> GetAllUser()
        {
            return await WorkScope.GetAll<User>()
                .Where(x => x.UserType != UserType.FakeUser)
                .Select(x => new ConfigITUserDto
                {
                    Id = x.Id,
                    FullName = x.Name + " " + x.Surname,
                    EmailAddress = x.EmailAddress,
                    Name = x.Name,
                    Surname = x.Surname
                })
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Surname)
                .ToListAsync();
        }

        [HttpDelete]
        public async Task Delete(long id)
        {
            var config = await WorkScope.GetAsync<ConfigIT>(id);
            if (config == null)
                throw new UserFriendlyException("Config not found!");

            await WorkScope.DeleteAsync(config);
        }

        private async Task<ConfigITDto> GetById(long id)
        {
            return await WorkScope.GetAll<ConfigIT>()
                .Where(x => x.Id == id)
                .Include(x => x.User)
                    .ThenInclude(x => x.Branch)
                .Include(x => x.User)
                    .ThenInclude(x => x.Position)
                .Select(x => new ConfigITDto
                {
                    Id = x.Id,
                    EmailAddress = x.User.EmailAddress,
                    AvatarPath = x.User.AvatarPath,
                    UserType = x.User.UserType,
                    UserLevel = x.User.UserLevel,
                    Branch = x.User.BranchOld,
                    FullName = x.User.Name + " " + x.User.Surname,
                    CreationTime = x.CreationTime,
                    BranchColor = x.User.Branch != null ? x.User.Branch.Color : null,
                    BranchDisplayName = x.User.Branch != null ? x.User.Branch.DisplayName : null,
                    PositionId = x.User.PositionId,
                    PositionColor = x.User.Position != null ? x.User.Position.Color : null,
                    PositionName = x.User.Position != null ? x.User.Position.ShortName : null,
                    UserId = x.UserId
                })
                .FirstOrDefaultAsync();
        }
    }
}
