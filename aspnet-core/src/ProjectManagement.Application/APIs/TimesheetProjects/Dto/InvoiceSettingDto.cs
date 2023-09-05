using ProjectManagement.APIs.Timesheets.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.TimesheetProjects.Dto
{
    public class InvoiceSettingDto
    {
        public float Discount { get; set; }
        public float TransferFee { get; set; }
        public string ProjectName { get; set; }
        public string ClientCode { get; set; }
        public string CurrencyCode { get; set; }
        public long InvoiceNumber { get; set; }
        public byte PaymentDueBy { get; set; }
        public List<IdNameDto> SubProjects { get; set; }
    }
}
