using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.CheckPointUserDetails.Dto
{
    [AutoMapTo(typeof(CheckPointUserDetail))]
    public class SelectCheckPointUserDetailDto : EntityDto<long>
    {
        public string CriteriaName { get; set; }
        public int? Score { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }

    }
}
