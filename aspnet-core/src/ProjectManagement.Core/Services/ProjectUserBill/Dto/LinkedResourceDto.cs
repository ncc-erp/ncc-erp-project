using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class LinkedResourcesDto
    {
        [Required]
        public long BillAccountId { get; set; }

        [Required]
        public List<long> UserIds { get; set; }
    }

    public class LinkedResourceDto
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public long BillAccountId { get; set; }
    }
}
