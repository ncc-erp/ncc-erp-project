using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class BaseInvoiceSettingDto
    {
        public long InvoiceNumber { get; set; }
        public float Discount { get; set; }

        public bool IsMainProjectInvoice { get; set; } = true;
        public long? MainProjectId { get; set; }
        public List<long> SubProjectIds { get; set; }
    }
}
