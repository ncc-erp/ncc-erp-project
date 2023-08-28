using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using NccCore.Anotations;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    [AutoMapTo(typeof(TimesheetProjectBill))]
    public class TimeSheetProjectBillDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public string AccountName { get; set; }
        public string BillRole { get; set; }
        public float BillRate { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }
        public float WorkingTime { get; set; }
        public ChargeType? ChargeType { get; set; }
    }
}
