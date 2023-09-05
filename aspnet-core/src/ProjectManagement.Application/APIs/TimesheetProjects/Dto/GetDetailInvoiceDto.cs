using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class GetDetailInvoiceDto
    {
        public long ClientId { get; set; }
        public string ClientName { get; set; }
        public int TotalProject { get; set; }
    }
}
