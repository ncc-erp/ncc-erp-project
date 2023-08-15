using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckPointUsers.Dto
{
    [AutoMapTo(typeof(CheckPointUser))]

    public class CheckPointUserInputDto : Entity<long>
    {
        public long PhaseId { get; set; }
        public long? ReviewerId { get; set; }
        public long? UserId { get; set; }
        public CheckPointUserType Type { get; set; }
        //public bool PhaseStatus { get; set; }

    }
}
