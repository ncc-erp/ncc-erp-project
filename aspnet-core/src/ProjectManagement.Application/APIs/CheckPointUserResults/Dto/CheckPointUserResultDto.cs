using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.CheckPointUserResults.Dto
{
    [AutoMapTo(typeof(CheckPointUserResult))]
    public class CheckPointUserResultDto : EntityDto<long>
    {
        public long PhaseId { get; set; }
        public long UserId { get; set; }
        public string UserName{ get; set; }
        public long ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public string UserNote { get; set; }
        public string PMNote { get; set; }
        public string FinalNote { get; set; }
        public UserLevel CurrentLevel { get; set; }
        public UserLevel ExpectedLevel { get; set; }
        public UserLevel NowLevel { get; set; }
        public int PMScore { get; set; }
        public int TeamScore { get; set; }
        public int ClientScore { get; set; }
        public int ExamScore { get; set; }
        public CheckPointUserResultStatus Status { get; set; }
        public IEnumerable<ResultTagDto> Tags { get; set; }
    }
}
