using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class MergeInvoiceDto
    {
        public long TimesheetId{ get; set; }
        public List<IsMergeInvoiceDto> MergeInvoice { get; set; }
    }

    public class IsMergeInvoiceDto
    {
        public long ClientId { get; set; }
        public bool isMergeInvoice { get; set; }
    }
}
