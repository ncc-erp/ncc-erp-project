using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectTimesheet.Dto
{
    public class ProjectUserBillDto
    {
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public string AccountName { get; set; }
        public float BillRate { get; set; }
        public string BillRole { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public ChargeType? ChargeType { get; set; }
        public long? CurrencyId { get; set; }
        public float Discount { get; set; }
        public float TransferFee { get; set; }
        public long LastInvoiceNumber { get; set; }
        public long? ParentInvoiceId { get; set; }

    }
}
