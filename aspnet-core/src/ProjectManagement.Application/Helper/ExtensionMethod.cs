using ProjectManagement.Services.Timesheet.Dto;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Helper
{
    public static class ExtensionMethod
    {
        /// <summary>
        /// Get Working day OT
        /// </summary>
        /// <param name="ot"></param>
        /// <param name="tsUser"></param>
        /// <returns></returns>
        public static double GetWorkingDayOT(TimesheetProjectBillOtTypeDto ot, TimesheetUser tsUser)
        {
            var otHours = (float)ot.Hours;
            double result;

            if ((tsUser.Mode == ExportInvoiceMode.MontlyToDaily && tsUser.ChargeType == ChargeType.Monthly) || tsUser.ChargeType == ChargeType.Daily)
            {
                result = otHours / tsUser.DefaultWorkingHours;
            }
            else if (tsUser.ChargeType == ChargeType.Hourly)
            {
                result = otHours;
            }
            else
            {
                result = (otHours / tsUser.DefaultWorkingHours) / tsUser.TimesheetWorkingDay;
            }

            return Math.Round(result, 3);
        }
    }
}
