using Abp.AutoMapper;
using Abp.Domain.Entities;
using ProjectManagement.Entities;
using System;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.PMReports.Dto
{
    [AutoMapTo(typeof(PMReport))]
    public class CreatePMReportDto : Entity<long>
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int Year { get; set; }
        public PMReportType Type { get; set; }
        public PMReportStatus PMReportStatus { get; set; }
        public DateTime? CanSendTime { get; set; }
        public DateTime? ExpiredTime { get; set; }
        public DateTime? LastReviewDate { get; set; }
    }
}
