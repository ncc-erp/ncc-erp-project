using ProjectManagement.NccCore.Helper;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Users.Dto
{
    public class EmployeeInformationDto
    {
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public UserType UserType { get; set; }
        public Job? Job { get; set; }
        public string RoleType => CommonUtil.UserTypeName(this.UserType);
        public string Position => CommonUtil.JobPositionName(Job);
        public string Branch { get; set; }
        public List<ProjectDTO> ProjectDtos { get; set; }
    }

    public class ProjectDTO
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string PmName { get; set; }
        public string PmUsername => UserHelper.GetUserName(this.PmEmail);
        public DateTime StartTime { get; set; }
        public ProjectUserRole PRole { get; set; }
        public string ProjectRole => Enum.GetName(typeof(ProjectUserRole), this.PRole);
        public string PmEmail { get; set; }
    }
}
