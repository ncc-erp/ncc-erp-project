using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.APIs.TS.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagement.APIs.TS
{
    public class TSAppService: ProjectManagementAppServiceBase
    {
        /*[AbpAllowAnonymous]
        [HttpPost]
        public void UpdateAvatarFromTimesheet(UpdateAvatarDto input)
        {
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
        }*/
        [AbpAllowAnonymous]
        [HttpPost]
        public void UpdateAllAvatarFromTimesheet(List<UpdateAvatarDto> input)
        {
            CheckSecurityCode();
            if (input.Count() == 0 || input == null)
            {
                Logger.Error($"input null or empty");
                return;
            }

            var emails = input.Select(x => x.EmailAddress).ToList();

            var users = GetUsers(emails);

            if (users == null || users.Count() == 0)
            {
                Logger.Error($"not found users with emails {emails}");
                return;
            }

            var usersAndAvatarPaths = (from user in users 
                                       join i in input on user.EmailAddress equals i.EmailAddress
                                       select new {
                                           User = user,
                                           AvatarPath = i.AvatarPath
                                       }).ToList();

            usersAndAvatarPaths.ForEach(x => x.User.AvatarPath = x.AvatarPath);

            CurrentUnitOfWork.SaveChanges();
        }

    }
}
