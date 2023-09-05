using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.Finance.Dto
{
    public class ResponseResultProjectDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<CreateInvoiceDto> SentInvoices { get; set; }
        public List<TotalMoneyByCurrencyDto> ListTotalMoneyByCurrency { get; set; }
    }
}
