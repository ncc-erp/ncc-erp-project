using Abp.Application.Services.Dto;
using NccCore.Anotations;

namespace ProjectManagement.APIs.Punishments.Dto
{
    public class GetPunishmentDto : EntityDto<long>
    {
        [ApplySearchAttribute]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Note { get; set; }
    }
}
