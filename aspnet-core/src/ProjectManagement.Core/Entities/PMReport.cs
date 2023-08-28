using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class PMReport : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        public int Year { get; set; }
        public PMReportType Type { get; set; }
        public PMReportStatus PMReportStatus { get; set; }
        public string Note { get; set; }
    }
}
