using System.Threading.Tasks;
using Abp.Application.Services;
using ProjectManagement.Sessions.Dto;

namespace ProjectManagement.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
