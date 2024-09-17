using ProjectManagement.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Projects.Dto
{
    public class GetBillInfoDto
    {
        public string BillRole { get; set; }
        public float BillRate { get; set; }
        public float HeadCount { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool isActive { get; set; }
        public bool isExpose { get; set; }
        public string FullName { get; set; }
    }

    public class ResourceInfo
    {
        public long ProjectId { get; set; }
        public string ProjectUserRole { get; set; }
        public string FullName { get; set; }
        public DateTime StartTime { get; set; } 
    }
}
