using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class ProjectUserBillAccountsDto
    {
        [Required]
        public long BillAccountId { get; set; }
        [Required]
        public long ProjectId { get; set; }
        public List<long> UserIds { get; set; }
    }

    public class ProjectUserBillAccountDto
    {
        [Required]
        public long BillAccountId { get; set; }
        [Required]
        public long ProjectId { get; set; }
        [Required]
        public long UserId { get; set; }
    }
}