using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ProjectManagement.Constants.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class Timesheet : FullAuditedEntity<long>, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        [MaxLength(255)]
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool IsActive { get; set; }
        [DefaultValue(false)]
        public bool CreatedInvoice { get; set; }
        public float? TotalWorkingDay { get; set; }
    }
}
