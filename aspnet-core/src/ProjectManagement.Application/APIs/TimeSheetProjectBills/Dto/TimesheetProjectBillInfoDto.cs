using Abp.Configuration;
using ProjectManagement.Configuration;
using ProjectManagement.Services.Timesheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimeSheetProjectBills.Dto
{
    public class TimesheetProjectBillInfoDto
    {
        public string UserFullName { get; set; }
        public string AccountName { get; set; }
        public string BillRole { get; set; }
        public float BillRate { get; set; }
        public float WorkingTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public ChargeType? ChargeType { get; set; }
        public string FullName => string.IsNullOrEmpty(AccountName) ? UserFullName : AccountName;
        public double TimeSheetWorkingDay { get; set; }
        public int DefaultWorkingHours { get; set; }
        public List<TimesheetProjectBillOtTypeDto> TimesheetProjectBillOtTypes { get; set; }
        public double Amount => GetWorkingTime() * BillRate;
        public double RoundAmount => Math.Round(Amount);
        private double GetWorkingTime()
        {
            var otHours = TimesheetProjectBillOtTypes?.Sum(item => (float)item.Hours * item.Multiplier) ?? 0;
            if (ChargeType == Constants.Enum.ProjectEnum.ChargeType.Daily)
            {
                return WorkingTime + otHours / DefaultWorkingHours;
            }

            if (ChargeType == Constants.Enum.ProjectEnum.ChargeType.Hourly)
            {
                return WorkingTime * DefaultWorkingHours + otHours;
            }

            return TimeSheetWorkingDay == 0 ? 0 : (WorkingTime + otHours / DefaultWorkingHours) / TimeSheetWorkingDay;
        }
    }
}
