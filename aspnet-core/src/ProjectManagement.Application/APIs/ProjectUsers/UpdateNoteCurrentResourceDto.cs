using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectUsers
{
    [AutoMapTo(typeof(ProjectUser))]
    public class UpdateNoteCurrentResourceDto : EntityDto<long>
    {
        public string Note { get; set; }
    }
}