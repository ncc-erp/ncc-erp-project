using NccCore.Paging;
using System.Collections.Generic;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class GetAllProjectUserBillDto
    {
        public long ProjectId { get; set; }
        public ChargeStatusFilter ChargeStatusFilter { get; set; }
        public List<long> LinkedResourcesFilter { get; set; }
        public List<string> ChargeRoleFilter { get; set; }
        public string SearchText { get; set; }
    }
}