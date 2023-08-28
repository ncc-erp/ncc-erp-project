using NccCore.Uitls;
using ProjectManagement.Entities;
using ProjectManagement.Helper;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProjectManagement.Constants.Enum.ClientEnum;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.Timesheet.Dto
{
    public class InvoiceData
    {
        public InvoiceGeneralInfo Info { get; set; }
        public List<TimesheetUser> TimesheetUsers { get; set; }
        public List<string> ProjectCodes { get; set; }

        private bool IsInvoiceHaveOneProject()
        {
            return TimesheetUsers.Select(s => s.ProjectName).Distinct().Count() == 1;
        }

        private string ProjectName()
        {
            return TimesheetUsers.Select(s => s.ProjectName).FirstOrDefault();
        }
        public string CurrencyName()
        {
            return TimesheetUsers.Select(s => s.CurrencyName).FirstOrDefault();
        }
        public string ExportFileName()
        {
            string projectName = IsInvoiceHaveOneProject() ? "_" + ProjectName() : "";
            var date = new DateTime(Info.Year, Info.Month, 1);

            return FilesHelper.SetFileName($"{Info.ClientName}{projectName}_Invoice{Info.InvoiceNumber}_{date.ToString("yyyyMM")}");
        }
        public string ExportFileNameAsPDF()
        {
            string projectName = IsInvoiceHaveOneProject() ? "_" + ProjectName() : "";
            var date = new DateTime(Info.Year, Info.Month, 1);

            return FilesHelper.SetFileNameAsPDF($"{Info.ClientName}{projectName}_Invoice{Info.InvoiceNumber}_{date.ToString("yyyyMM")}");
        }
    }

    public class InvoiceGeneralInfo
    {
        public string ClientName { get; set; }
        public float TransferFee { get; set; }
        public float Discount { get; set; }
        public long InvoiceNumber { get; set; }

        public string ClientAddress { get; set; }
        public string PaymentInfo { get; set; }
        public byte PaymentDueBy { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public InvoiceDateSetting InvoiceDateSetting { get; set; }

        public string[] PaymentInfoArr()
        {
            return PaymentInfo.Split("\n");
        }
        public string InvoiceDateStr()
        {
            var date = new DateTime(Year, Month, 1).AddMonths(1);
            if (InvoiceDateSetting == InvoiceDateSetting.LastDateThisMonth)
            {
                date = date.AddDays(-1);
            }

            return DateTimeUtils.FormatDateToInvoice(date);
        }

        public string PaymentDueByStr()
        {
            var date = new DateTime(Year, Month, 1).AddMonths(2).AddDays(-1);
            if (PaymentDueBy >= 1 && PaymentDueBy <= 100)
            {
                int months = PaymentDueBy / 30 + 1;
                try
                {
                    date = new DateTime(Year, Month, PaymentDueBy % 30).AddMonths(months);
                }
                catch
                {
                    date = new DateTime(Year, Month, 1).AddMonths(months + 1).AddDays(-1);
                }
            }
            if(PaymentDueBy > CommonUtil.LastDateNextThan2Month)
            {
                date = new DateTime(Year, Month, 1).AddMonths(PaymentDueBy%100).AddDays(-1);
            }    

            return DateTimeUtils.FormatDateToInvoice(date);
        }
    }

    public class TimesheetUser
    {
        public long UserId { get; set; }
        public string UserFullName { get; set; }
        public string AccountName { get; set; }
        public string EmailAddress { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public double WorkingDay { get; set; }
        public ChargeType ChargeType { get; set; }
        public string CurrencyName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double BillRate { get; set; }
        public int DefaultWorkingHours { get; set; }
        public ExportInvoiceMode Mode { get; set; }
        public double TimesheetWorkingDay { get; set; }
        public string FullName => string.IsNullOrEmpty(AccountName) ? UserFullName : AccountName;
        public double BillRateDisplay => (Mode == ExportInvoiceMode.MontlyToDaily && ChargeType == ChargeType.Monthly) ? BillRate / TimesheetWorkingDay : BillRate;
        public double WorkingDayDisplay
        {
            get
            {
                if ((Mode == ExportInvoiceMode.MontlyToDaily && ChargeType == ChargeType.Monthly) || ChargeType == ChargeType.Daily)
                {
                    return WorkingDay;
                }

                if (ChargeType == ChargeType.Hourly)
                {
                    return WorkingDay * DefaultWorkingHours;
                }

                return WorkingDay / TimesheetWorkingDay;
            }
        }
        public string ChargeTypeDisplay
        {
            get
            {
                if ((Mode == ExportInvoiceMode.MontlyToDaily && ChargeType == ChargeType.Monthly) || ChargeType == ChargeType.Daily)
                {
                    return "Day";
                }

                if (ChargeType == ChargeType.Hourly)
                {
                    return "Hour";
                }

                return "Month";
            }
        }
        public double LineTotal
        {
            get
            {
                return WorkingDayDisplay * BillRateDisplay;
            }
        }
    }

    public class TimesheetTaxDto
    {
        public List<DateTime> ListWorkingDay { get; set; }
        public List<TimesheetDetail> ListTimesheet { get; set; }
    }

    public class TimesheetDetailUser
    {
        public int ProjectNumber { get; set; }
        public string FullName { get; set; }
        public string ProjectName { get; set; }
        public List<TimesheetDetail> TimesheetDetails { get; set; }
    }
    public class TimesheetDetail
    {
        public DateTime DateAt { get; set; }
        public double ManDay { get; set; }
        public string TaskName { get; set; }
        public string Note { get; set; }
        public string EmailAddress { get; set; }
        public string ProjectCode { get; set; }
    }
    public enum ExportInvoiceMode : byte
    {
        Normal = 0,
        MontlyToDaily = 1
    }
    #region Finfast Integrate
    public class InvoiceGeneralInfoForFinfast
    {
        public long ProjectId { get; set; }
        public string ClientName { get; set; }
        public float TransferFee { get; set; }
        public float Discount { get; set; }
        public long InvoiceNumber { get; set; }
        public string ProjectCode { get; set; }
        public string ClientAddress { get; set; }
        public string PaymentInfo { get; set; }
        public byte PaymentDueBy { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public double TimesheetWorkingDay { get; set; }
        public InvoiceDateSetting InvoiceDateSetting { get; set; }
        public string AccountCode { get; set; }
        public string[] PaymentInfoArr()
        {
            return PaymentInfo.Split("\n");
        }
        public string InvoiceDateStr()
        {
            var date = new DateTime(Year, Month, 1).AddMonths(1);
            if (InvoiceDateSetting == InvoiceDateSetting.LastDateThisMonth)
            {
                date = date.AddDays(-1);
            }

            return DateTimeUtils.FormatDateToInvoice(date);
        }

        public string PaymentDueByStr()
        {
            var date = new DateTime(Year, Month, 1).AddMonths(2).AddDays(-1);
            if (PaymentDueBy >= 1 && PaymentDueBy <= 100)
            {
                int months = PaymentDueBy / 30 + 1;
                try
                {
                    date = new DateTime(Year, Month, PaymentDueBy % 30).AddMonths(months);
                }
                catch
                {
                    date = new DateTime(Year, Month, 1).AddMonths(months + 1).AddDays(-1);
                }
            }
            if (PaymentDueBy > CommonUtil.LastDateNextThan2Month)
            {
                date = new DateTime(Year, Month, 1).AddMonths(PaymentDueBy%100).AddDays(-1);
            }

            return DateTimeUtils.FormatDateToInvoice(date);
        }
        public DateTime PaymentDueByDate()
        {
            return DateTimeUtils.PaymentDueByDate(Year, Month, PaymentDueBy);
        }
    }
    public class InvoiceDataForFinfast
    {
        public InvoiceGeneralInfoForFinfast Info { get; set; }
        public List<TimesheetUserForFinfast> TimesheetUsers { get; set; }
        public List<string> ProjectCodes { get; set; }
        public string CurrencyCode => TimesheetUsers.Select(s => s.CurrencyName).FirstOrDefault();
        public string ClientCode => Info.AccountCode;
        private double NetTotal => TimesheetUsers.Sum(s => s.LineTotal);
        public double InvoiceTotal => this.NetTotal - (Info.Discount * this.NetTotal) / 100;
    }
    public class TimesheetUserForFinfast : TimesheetUser
    {
        public string CurrencyCode { get; set; }
    }
    #endregion
}
