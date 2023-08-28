using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    [AutoMapTo(typeof(ProjectUserBill))]
    public class ProjectUserBillDto : EntityDto<long>
    {
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public string BillRole { get; set; }
        public float BillRate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Note { get; set; }
        public string shadowNote { get; set; }
        public bool isActive { get; set; }
        public string AccountName { get; set; }
        public ChargeType? ChargeType { get; set; }
    }
}
