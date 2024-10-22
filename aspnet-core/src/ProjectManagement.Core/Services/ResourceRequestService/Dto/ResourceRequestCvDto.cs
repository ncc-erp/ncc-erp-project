using Abp.Application.Services.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ResourceRequestService.Dto
{
    public class ResourceRequestCvDto : EntityDto<long>
    {
        public long UserId { get; set; }
        public UserBaseDto User { get; set; }
        public CVStatus? Status { get; set; }
        public long ResourceRequestId { get; set; }
        public string CVName { get; set; }
        public string CVPath { get; set; }
        public string LinkCVPath { get => FileUtils.FullFilePath(CVPath); }
        public string Note { get; set; }
        public double? KpiPoint { get; set; }
        public DateTime? InterviewDate { get; set; }
        public DateTime? SendCVDate { get; set; }
        public long? CvStatusId { get; set; }
        public CvStatusDto CvStatus { get; set; }
    }

    public class CvStatusDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string Color { get; set; }
    }
}
