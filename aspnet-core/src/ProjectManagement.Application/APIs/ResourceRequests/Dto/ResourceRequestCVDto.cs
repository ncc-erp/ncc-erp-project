using Abp.Application.Services.Dto;
using Abp.AutoMapper;

using ProjectManagement.Entities;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Users.Dto;
using ProjectManagement.Services.ResourceRequestService.Dto;
namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    [AutoMapTo(typeof(ResourceRequestCV))]
    public class ResourceRequestCVDto : EntityDto<long>
    {

        public long UserId { get; set; }
        public UserBaseDto User { get; set; }
        public CVStatus? Status { get; set; }
        public long ResourceRequestId { get; set; }
        public string CVName { get; set; }
        public string CVPath { get; set; }
        public string LinkCVPath { get => FileUtils.FullFilePath(CVPath); }
        public string Note { get; set; }
        public double? KpiPoint { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime? SendCVDate { get; set; }

    }
}
