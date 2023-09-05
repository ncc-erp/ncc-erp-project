using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.CheckPointUserDetails.Dto
{
    [AutoMapTo(typeof(CheckPointUserDetail))]
    public class CheckPointUserDetailDto : EntityDto<long>
    {
        public long CheckPointUserId { get; set; }
        public long CriteriaId { get; set; }
        public int? Score { get; set; }
        public string Note { get; set; }
    }
}
