using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Currencies.Dto
{
    [AutoMapTo(typeof(Currency))]
    public class CurrencyDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string InvoicePaymentInfo { get; set; }
    }
}
