using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.HRMv2.Dto
{
    public class UpdateUserWorkingStatusFromHRMV2Dto
    {
        public string EmailAddress { get; set; }
     
        public DateTime DateAt { get; set; }
    }
}
