using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.PMReports.Dto
{
    public class TotalFutureUseDto
    {
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public int Total { get; set; }
    }
}
