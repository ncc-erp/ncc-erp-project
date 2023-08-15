using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.ProjectUserBills.Dto
{
    [AutoMapTo(typeof(ProjectUserBill))]
    public class UpdateNoteDto : EntityDto<long>
    {
        public string Note { get; set; }
    }
}
