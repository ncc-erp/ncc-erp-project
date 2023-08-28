using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class AddSubInvoiceDto
    {
        public long ParentInvoiceId { get; set; }
        public long SubInvoiceId { get; set; }
    }

    public class AddSubInvoicesDto
    {
        public long ParentInvoiceId { get; set; }
        public List<long> SubInvoiceIds { get; set; }
    }
}
