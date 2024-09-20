using NccCore.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    public class BillInfoResultDto
    {
        public GridResult<BillInfoDto> GridResult { get; set; }
        public float TotalHeadCount { get; set; }
    }
}
