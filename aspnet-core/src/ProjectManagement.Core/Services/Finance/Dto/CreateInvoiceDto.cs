using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.Finance.Dto
{
    public class CreateInvoiceDto
    {
        public string NameInvoice { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClientCode { get; set; }
        public short Month { get; set; }
        public int Year { get; set; }
        public double CollectionDebt => InvoiceMoney - TransferFee;
        public double InvoiceMoney { get; set; }
        public string CurrencyCode { get; set; }
        public float TransferFee { get; set; }
        public DateTime Deadline { get; set; }
    }

    public class InvoiceDetailDto
    {
        public string ProjectName { get; set; }
        public long FileId { get; set; }
        public string LinkFile { get; set; }
    }
}
