using Amazon.S3.Model;
using ProjectManagement.Services.ResourceRequestService.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.ResourceRequests.Dto
{
    public class UpdateResouceRequestCVStatus
    {
        public long ResourceRequestId { get; set; }
        public long ResourceRequestCVId {  get; set; }
        public CVStatus? Status { get; set; }

    }
    public class UpdateStatusResult { 
    
        public UpdateResouceRequestCVStatus updateResouceRequestCVStatus { set; get; }
        public GetResourceRequestDto getResourceRequestDto { get; set; }

    }

}
