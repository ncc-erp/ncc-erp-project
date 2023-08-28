using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckPointUserDetails.Dto
{
    [AutoMapTo(typeof(CheckPointUserDetail))]
    public class HistoryDto : EntityDto<long>
    {
        public string PhaseName { get; set; }
        public string Reviewer { get; set; }
        public CheckPointUserType Type { get; set; }
        public int? Score { get; set; }
        public string Note { get; set; }
    }
}
