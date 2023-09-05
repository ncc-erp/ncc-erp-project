using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckPointUserDetails.Dto
{
    [AutoMapTo(typeof(CheckPointUser))]
    public class InputCriteriaDto : EntityDto<long>
    {
        public long PhaseId { get; set; }
        public long MemberId { get; set; }
        public List<CheckPointUserDetailDto> CheckPointUserDetails { get; set; }
        public string Note { get; set; }
        public int? Score { get; set; }
        public UserLevel RequestLevel { get; set; }
    }
}
