using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.CheckPointUserResults.Dto
{
    [AutoMapTo(typeof(CheckPointUserResultTag))]
    public class ResultTagDto : EntityDto<long>
    {
        public long CheckPointUserResultId { get; set; }
        public long TagId { get; set; }
        public string TagName { get; set; }
    }
}
