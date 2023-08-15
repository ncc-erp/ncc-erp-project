using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class UpdateInvoiceDto: BaseInvoiceSettingDto
    {
        public long ProjectId { get; set; }
        
    }
}
