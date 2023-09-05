using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class ProjectRateDto
    {
        public string CurrencyName { get; set; }
        public bool? IsCharge { get; set; }
        public ChargeType? ChargeType { get; set; }
    }
}
