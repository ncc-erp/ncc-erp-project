using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Microsoft.AspNetCore.Http;
using ProjectManagement.Entities;

namespace ProjectManagement.APIs.Punishments.Dto
{
    [AutoMapTo(typeof(Punishment))]
    public class PunishmentDto : EntityDto<long>
    {
        public IFormFile File { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Note { get; set; }
    }
}
