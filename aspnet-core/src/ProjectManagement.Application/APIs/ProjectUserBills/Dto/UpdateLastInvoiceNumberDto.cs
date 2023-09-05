using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class UpdateLastInvoiceNumberDto
    {
        public long ProjectId { get; set; }
        public long LastInvoiceNumber { get; set; }
    }
}
