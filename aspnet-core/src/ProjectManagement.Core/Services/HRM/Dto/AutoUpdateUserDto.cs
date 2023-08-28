using Abp.AutoMapper;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.HRM.Dto
{
    [AutoMapTo(typeof(User))]
    public class AutoUpdateUserDto
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string EmailAddress { get; set; }
        public string UserCode { set; get; }
        public UserType UserType { get; set; }
        public UserLevel UserLevel { get; set; }
        public Branch? Branch { get; set; }
        public long? BranchId { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public Job? Job { get; set; }
        public Status Status { get; set; }
        public DateTime? StartDayOff { get; set; }
    }

    public enum Status
    {
        Working = 0,
        Pausing = 1,
        Quit = 2,
        MaternityLeave = 3
    }
}
