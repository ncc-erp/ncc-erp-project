using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using ProjectManagement.Constants;
using ProjectManagement.Constants.Enum;
using ProjectManagement.Entities;
using ProjectManagement.Services.Timesheet.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class InputExportInvoiceDto
    {
        public long TimesheetId { get; set; }
        public List<long> ProjectIds { get; set; }
        public ExportInvoiceMode Mode { get; set; }
    }
    public class InputSendInvoiceForFinfastDto
    {
        public long MainInvoiceProjectId { get; set; }
        public long TimesheetId { get; set; }
        public List<long> ProjectIds { get; set; }
        public IEnumerable<TimesheetProjectBillGeneralInfoForFinfast> TimesheetProjectBills { get; set; }
        public IEnumerable<InvoiceGeneralInfoForFinfast> TimesheetProjects { get; set; }
    }
    public class TimesheetProjectBillGeneralInfoForFinfast
    {
        public long ProjectId { get; set; }
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
        public string CurrencyCode { get; set; }

    }
}
