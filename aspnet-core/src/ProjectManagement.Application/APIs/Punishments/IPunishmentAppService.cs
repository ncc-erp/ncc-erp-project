using Abp.Application.Services;
using Abp.Dependency;
using NccCore.Paging;
using ProjectManagement.APIs.Punishments.Dto;
using ProjectManagement.Entities;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.Punishments
{
    public interface IPunishmentAppService : IApplicationService, ITransientDependency
    {
        Task<GridResult<GetPunishmentDto>> GetAllPaging(GridParam input);
        Task<long> Create(PunishmentDto input);
        Task<Punishment> Update(PunishmentDto input);
        Task Delete(long id);
        Task<PunishmentDto> GetPunishment(long id);
        Task<object> DownloadPunishmentFileAsync(long id);
    }
}
