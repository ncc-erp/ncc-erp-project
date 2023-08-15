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
}