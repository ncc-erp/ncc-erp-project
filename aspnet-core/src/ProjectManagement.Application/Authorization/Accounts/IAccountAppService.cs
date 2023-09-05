using System.Threading.Tasks;
using Abp.Application.Services;
using ProjectManagement.Authorization.Accounts.Dto;

namespace ProjectManagement.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
