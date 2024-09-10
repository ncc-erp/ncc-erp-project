using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateResouceRequestCVKpiPoint
    {
        public long ResourceRequestCVId { get; set; }
        public double KpiPoint {  get; set; }
    }
}
