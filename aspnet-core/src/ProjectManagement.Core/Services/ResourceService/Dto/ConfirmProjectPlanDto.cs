using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.Services.ResourceService.Dto
{
    public class ConfirmProjectPlanDto
    {
        public long ProjectUserId { get; set; }
        public DateTime ConfirmDate { get; set; }
    }
}
