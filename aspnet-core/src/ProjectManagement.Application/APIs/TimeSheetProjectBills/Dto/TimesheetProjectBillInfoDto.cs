using ProjectManagement.Services.Timesheet.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public double RoundAmount => Math.Round(Amount, 2);
        private double GetWorkingTime()
        {
            var otHours = TimesheetProjectBillOtTypes?.Sum(item => (double)item.Hours * (double)item.Multiplier) ?? 0;
            double result;
            if (ChargeType == Constants.Enum.ProjectEnum.ChargeType.Daily)
            {
                result = WorkingTime + otHours / DefaultWorkingHours;
            }
            else if (ChargeType == Constants.Enum.ProjectEnum.ChargeType.Hourly)
            {
                result = WorkingTime * DefaultWorkingHours + otHours;
            }
            else
            {
                result = TimeSheetWorkingDay == 0 ? 0 : (WorkingTime + otHours / DefaultWorkingHours) / TimeSheetWorkingDay;
            }
            return result;
        }
    }
}
