using Abp.Application.Services.Dto;

namespace ProjectManagement.Services.ResourceRequestService.Dto
{
    public class UpdateConfidenceLevelDto: EntityDto<long>
    {
        public float ConfidenceLevel { get; set; }
    }
    
    public class UpdateHeadCountDto: EntityDto<long>
    {
        public float HeadCount { get; set; }
    }
}