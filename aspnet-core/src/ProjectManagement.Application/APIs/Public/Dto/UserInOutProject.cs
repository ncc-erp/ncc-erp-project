using System;
using System.Collections.Generic;

namespace ProjectManagement.APIs.Public.Dto
{
    public class UserInOutProject
    {
        public string EmailAddress { get; set; }
        public List<TimeJoinOut> ListTimeInOut { get; set; }
    }

    public class TimeJoinOut
    {
        public DateTime DateAt { get; set; }
        public bool IsJoin { get; set; }
    }

    public class GetUserTempInProject
    {
        public string EmailAddress { get; set; }
    }

    public class ResultGetUserTempInProject
    {
        public string Code { get; set; }
        public List<GetUserTempInProject> ListUserTempInProject { get; set; }
    }

    public class PMsOfUsersDto
    {
        public string UserEmail { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string PMEmail { get; set; }
        public string PMFullName { get; set; }
    }
}