using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ProjectManagement.Constants.Enum.ClientEnum;

namespace ProjectManagement.Entities
{
    public class Client : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }
        public string Address { get; set; }
        public InvoiceDateSetting InvoiceDateSetting { get; set; }
        public byte PaymentDueBy { get; set; }
        public float TransferFee { get; set; }
    }
}
