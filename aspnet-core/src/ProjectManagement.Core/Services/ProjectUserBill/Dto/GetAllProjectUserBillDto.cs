using NccCore.Paging;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class GetAllProjectUserBillDto : GridParam
    {
        public long ProjectId { get; set; }
        public ChargeStatus ChargeStatus { get; set; }
        public ChargeType ChargeType { get; set; }
        public List<string> ChargeNameFilter { get; set; }
        public List<string> ChargeRoleFilter { get; set; }
    }
}