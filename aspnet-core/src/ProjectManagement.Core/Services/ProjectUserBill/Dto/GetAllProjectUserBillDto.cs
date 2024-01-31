using NccCore.Paging;
using System.Collections.Generic;

namespace ProjectManagement.Services.ProjectUserBill.Dto
{
    public class GetAllProjectUserBillDto : GridParam
    {
        public long ProjectId { get; set; }
        public IDictionary<string, SortDirection> SortParams { get; set; }
    }
}