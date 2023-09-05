using Abp.Configuration;
using ProjectManagement.Configuration;
using System;
using System.Collections.Generic;
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
        public string Description { get; set; }
        public string Currency { get; set; }
        public ChargeType? ChargeType { get; set; }
        public string FullName => string.IsNullOrEmpty(AccountName) ? UserFullName : AccountName;
        public double TimeSheetWorkingDay { get; set; }
        public int DefaultWorkingHours { get; set; }
        public double Amount => GetWorkingTime() * BillRate;
        public double RoundAmount => Math.Round(Amount);
        private double GetWorkingTime()
        {
            if (ChargeType == Constants.Enum.ProjectEnum.ChargeType.Daily)
            {
                return WorkingTime;
            }

            if (ChargeType == Constants.Enum.ProjectEnum.ChargeType.Hourly)
            {
                return WorkingTime * DefaultWorkingHours;
            }

            return TimeSheetWorkingDay == 0 ? 0 : WorkingTime / TimeSheetWorkingDay;
        }
    }
}
