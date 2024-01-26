using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class ExportAllProjectInvoiceDto
    {
        public string Currency { get; set; }

        public List<ProjectInvoiceForExportDto> Clients { get; set; }

        public double Total => Clients.Sum(s => s.Total);
    }

    //Sheet Project
    public class NewStopProJectInforDto
    {
        public string ProjectName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class ClientNewStopProJectInforDto
    {
        public string ClientName { get; set; }

        public List<NewStopProJectInforDto> ProjectInfors { get; set; }
    }

    public class NewAndStopProjectDto
    {
        public List<ClientNewStopProJectInforDto> NewProject { get; set; }

        public List<ClientNewStopProJectInforDto> StopProject { get; set; }
    }

    //Sheet AccountsNotWorkingFull
    public class AccountsNotWorkingFullInfoDto
    {
        public string ProjectName { get; set; }

        public string AccountName { get; set; }

        //Số ngày làm thực tế
    }
    public class ClientAccountsNotWorkingFullDto
    {
        public string ClientName { get; set; }

        public List<AccountsNotWorkingFullInfoDto> StopProject { get; set; }
    }

    public class AccountsNotWorkingFullDto
    {
        public List<ClientAccountsNotWorkingFullDto> NewProject { get; set; }

        public List<ClientAccountsNotWorkingFullDto> StopProject { get; set; }
    }

    //Sheet Accouunts Change
    public class AccountsChangeInforDto
    {
        public string ProjectName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class ClientAccountsChangeInforDto
    {
        public string ClientName { get; set; }

        public List<AccountsChangeInforDto> AccountsChangeInfors { get; set; }
    }

    public class AccountsChangeDto
    {
        public List<ClientAccountsChangeInforDto> AccountIncreased { get; set; }

        public List<ClientAccountsChangeInforDto> AccountReduced { get; set; }
    }
}