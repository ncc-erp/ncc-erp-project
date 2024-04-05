using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using NccCore.Anotations;
using Newtonsoft.Json;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class UserShortInfoDto
    {
        public long Id { get; set; }
        public string EmailAddress { get; set; }
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public string FullName { get; set; }
        public long? BranchId { get; set; }
        public string BrandName { get; set; }
        [JsonIgnore]
        public string EmailWithoutDomain => !string.IsNullOrEmpty(EmailAddress) ? EmailAddress.Split('@')[0] : string.Empty;
    }
}
