using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class ProjectUserBillAccountsDto
    {
        public long BillAccountId { get; set; }
        public long ProjectId { get; set; }
        public List<long> UserIds { get; set; }
    }
}
