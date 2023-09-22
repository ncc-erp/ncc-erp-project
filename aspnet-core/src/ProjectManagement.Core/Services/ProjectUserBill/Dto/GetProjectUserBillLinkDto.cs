using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class GetProjectUserBillLinkDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public long UserBillAccountId { get; set; }
    }

    public class CreateProjectUserBillLinkDto
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long ProjectId { get; set; }
        [Required]
        public long UserBillAccountId { get; set; }
    }
}
