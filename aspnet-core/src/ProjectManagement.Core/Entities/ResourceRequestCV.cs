using Abp.Domain.Entities.Auditing;
using ProjectManagement.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Entities
{
    public class ResourceRequestCV : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
        public CVStatus? Status { get; set; }
        public long ResourceRequestId { get; set; }
        public string CVName { get; set; }
        public string CVPath { get; set; }
        public string Note { get; set; }
        public double KpiPoint { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime? SendCVDate { get; set; }
        public long? CvStatusId { get; set; }
        [ForeignKey(nameof(CvStatusId))]
        public CvStatus CvStatus { get; set; }
    }

}
