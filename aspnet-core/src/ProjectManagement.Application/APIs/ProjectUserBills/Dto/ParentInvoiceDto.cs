using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class ParentInvoiceDto
    {
        public long ProjectId { get; set; }
        public long? ParentId { get; set; }
        public string? ParentName { get; set; }
        public bool IsMainInvoice => !ParentId.HasValue ? true : false;
        public IEnumerable<SubInvoiceDto> SubInvoices { get; set; }
    }
    public class SubInvoiceDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
        public long? ParentId { get; set; }
        public string? ParentName { get; set; }
    }
    public class ProjectInvoiceDto
    {
        public long ProjectId { get; set; }
        public string Name { get; set; }
        public long? ParentInvoiceId { get; set; }
        public long? ClientId { get; set; }
    }
}
