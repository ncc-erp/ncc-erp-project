﻿using Abp.Timing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using ProjectManagement.Utils;

namespace NccCore.Uitls
{
    public class DateTimeUtils
    {
        // All now function use Clock.Provider.Now
        public static DateTime GetNow()
        {
            return Clock.Provider.Now;
        }

        public static string ToString(DateTime dateTime)
        {
            return dateTime.ToString("dd/MM/yyyy");
        }

        public static DateTime FirstDayOfMonth(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public static DateTime LastDayOfMonth(DateTime date)
        {
            return FirstDayOfMonth(date).AddMonths(1).AddDays(-1);
        }

        public static List<DateTime> GetListMonth(DateTime startDate, DateTime endDate)
        {
            var date = FirstDayOfMonth(startDate);
            var result = new List<DateTime>();
            while (date <= endDate)
            {
                result.Add(date);
                date = date.AddMonths(1);
            }
            return result;
        }

        public static string GetMonthName(DateTime date)
        {
            return date.ToString("MM-yyyy");
        }
        public static string GetFullNameOfMonth(int month)
        {
            return CultureInfo.CurrentCulture.
                DateTimeFormat.GetMonthName
                (month);
        }

        public static string FormatDateToInvoice(DateTime date)
        {
            var s = date.ToString("D", CultureInfo.GetCultureInfo("en-US"));
            return s.Substring(s.IndexOf(",") + 2);
        }

        public static DateTime PaymentDueByDate(int year, int month, int paymentDueBy)
        {
            var date = new DateTime(year, month, 1).AddMonths(2).AddDays(-1);
            if (paymentDueBy >= 1 && paymentDueBy <= 100)
            {
                int months = paymentDueBy / 30 + 1;
                try
                {
                    date = new DateTime(year, month, paymentDueBy % 30).AddMonths(months);
                }
                catch
                {
                    date = new DateTime(year, month, 1).AddMonths(months + 1).AddDays(-1);
                }
            }
            if (paymentDueBy > CommonUtil.LastDateNextThan2Month)
            {
                date = new DateTime(year, month, 1).AddMonths(paymentDueBy % 100).AddDays(-1);
            }
            return date;
        }
        public static long DateDiff(DateTime date1, DateTime date2)
        {
            return date1.Ticks - date2.Ticks;
        }

        public static long DateDiffAbs(DateTime date1, DateTime date2)
        {
            return Math.Abs(DateDiff(date1, date2));
        }

        public static DateTime MinDate(DateTime date1, DateTime date2)
        {
            return date1.Ticks < date2.Ticks ? date1 : date2; ;
        }


        public static string yyyyMM(int year, int month)
        {
            return new DateTime(year, month, 15).ToString("yyyy-MM");
        }

        public static string NowToyyyyMMddHHmmssfff()
        {
            return Clock.Provider.Now.ToString("yyyyMMddHHmmssfff");
        }
        public static int GetHourNow()
        {
            return Clock.Provider.Now.Hour;
        }

        public static int GetMinuteNow()
        {
            return Clock.Provider.Now.Minute;
        }

        public static DateTime ConvertToUtcTime(int year, int month, int day, int hour, int minute)
        {
            try
            {
                // Get the current year and month, -1 : get current
                int currentYear = year == -1 ? DateTime.Now.Year : year;
                int currentMonth = month == -1 ? DateTime.Now.Month : month;
                int currentDay = day == -1 ? DateTime.Now.Day : day;
                int currentHour = hour == -1 ? DateTime.Now.Hour : hour;
                int currentMinute = minute == -1 ? DateTime.Now.Minute : minute;
                // Create a DateTime with the given day and hour in the local timezone
                DateTime localTime = new DateTime(currentYear, currentMonth, currentDay, currentHour, currentMinute, 0, DateTimeKind.Local);
                // Convert local time to UTC
                return localTime.ToUniversalTime();
            }
            catch (Exception)
            {
                return new DateTime().ToUniversalTime();
            }

        }

    }
}
