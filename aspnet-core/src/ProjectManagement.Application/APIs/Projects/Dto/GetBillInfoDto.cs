using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class GetBillInfoDto
    {
        public string BillRole { get; set; }
        public float BillRate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool isActive { get; set; }
        public string FullName { get; set; }
    }
}
