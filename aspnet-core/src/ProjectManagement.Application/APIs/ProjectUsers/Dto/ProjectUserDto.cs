using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUsers.Dto
{
    [AutoMapTo(typeof(ProjectUser))]
    public class ProjectUserDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public ProjectUserRole ProjectRole { get; set; }
        public byte AllocatePercentage { get; set; }
        public DateTime StartTime { get; set; }
        public ProjectUserStatus Status { get; set; }
        public bool IsExpense { get; set; }
        public long? ResourceRequestId { get; set; }
        public long PMReportId { get; set; }
        public string Note { get; set; }
        public bool IsFutureActive { get; set; }
    }
}
