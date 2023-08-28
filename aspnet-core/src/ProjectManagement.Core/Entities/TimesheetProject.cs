using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagement.Entities
{
    public class TimesheetProject : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [ForeignKey(nameof(ProjectId))]
        public Project Project { get; set; }
        public long ProjectId { get; set; }
        [MaxLength(1000)]
        public string FilePath { get; set; }
        [ForeignKey(nameof(TimesheetId))]
        public Timesheet Timesheet { get; set; }
        public long TimesheetId { get; set; }
        [MaxLength(10000)]
        public string Note { get; set; }
        public string ProjectBillInfomation { get; set; }
        public string HistoryFile { get; set; }
        public bool? IsComplete { get; set; }

        public long InvoiceNumber { get; set; }
        public float TransferFee { get; set; }
        public float Discount { get; set; }
        public float WorkingDay { get; set; }
        public long? ParentInvoiceId { get; set; }
        public bool IsActive { get; set; }
    }
}