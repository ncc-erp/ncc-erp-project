using Abp.Application.Services.Dto;

namespace ProjectManagement.Services.ResourceManager
{
    public class GetProjectAllResourceDto
    {
        public long ProjectId { get; set; }
        public string ProjectName { get; set; }
    }
}