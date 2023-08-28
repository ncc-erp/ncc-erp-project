using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Authorization.Users;
using Abp.Extensions;
using ProjectManagement.Entities;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Authorization.Users
{
    public class User : AbpUser<User>
    {
        public const string DefaultPassword = "123qwe";

        public static string CreateRandomPassword()
        {
            return Guid.NewGuid().ToString("N").Truncate(16);
        }

        public static User CreateTenantAdminUser(int tenantId, string emailAddress)
        {
            var user = new User
            {
                TenantId = tenantId,
                UserName = AdminUserName,
                Name = AdminUserName,
                Surname = AdminUserName,
                EmailAddress = emailAddress,
                Roles = new List<UserRole>()
            };

            user.SetNormalizedNames();

            return user;
        }

        [MaxLength(256)]
        public string UserCode { set; get; }
        public string AvatarPath { get; set; }
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public ProjectManagement.Constants.Enum.ProjectEnum.Branch BranchOld { get; set; }
        public DateTime? DOB { get; set; }
        public long? KomuUserId { get; set; }
        public int? StarRate { get; set; }
        public Job? Job { get; set; }

        [MaxLength(3000)]
        public string PoolNote { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<UserSkill> UserSkills { get; set; }

        public long? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public ProjectManagement.Entities.Branch Branch { get; set; }

        public long? PositionId { get; set; }

        [ForeignKey(nameof(PositionId))]
        public ProjectManagement.Entities.Position Position { get; set; }

    }
}
