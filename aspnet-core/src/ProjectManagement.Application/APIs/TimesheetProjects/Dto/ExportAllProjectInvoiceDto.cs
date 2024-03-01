using Amazon.S3.Model;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using ProjectManagement.Entities;
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

    //Sheet Accounts Not Working Full
    public class AccountsNotWorkingFullDto
    {
        public float WorkingTime { get; set; }

        public string FullName { get; set; }
    }

    public class ClientAccountsNotWorkingFullDto
    {
        public string ClientName { get; set; }

        public List<ProjectAccountsNotWorkingFullDto> ProjectsInfo { get; set; }
    }

    public class ProjectAccountsNotWorkingFullDto
    {
        public string ProjectName { get; set; }

        public List<AccountsNotWorkingFullDto> AccountsInfo { get; set; }
    }

    //Sheet Projects Change
    public class ProjectsChangeInforDto
    {
        public string ProjectName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }

    public class ClientProjectsChangeInforDto
    {
        public string ClientName { get; set; }

        public List<ProjectsChangeInforDto> ProjectInfors { get; set; }
    }

    public class ProjectsChangeDto
    {
        public List<ClientProjectsChangeInforDto> NewProject { get; set; }

        public List<ClientProjectsChangeInforDto> StopProject { get; set; }
    }

    //Sheet Accounts Change
    public class AccountsChangeInforDto
    {
        public string FullName { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }
    }

    public class ClientAccountsChangeInforDto
    {
        public string ClientName { get; set; }

        public List<ProjectAccountsChangeInforDto> ProjectsInfor { get; set; }
    }

    public class ProjectAccountsChangeInforDto
    {
        public string ProjectName { get; set; }

        public List<AccountsChangeInforDto> NewAccounts { get; set; }
        public List<AccountsChangeInforDto> StopAccounts { get; set; }
    }
}