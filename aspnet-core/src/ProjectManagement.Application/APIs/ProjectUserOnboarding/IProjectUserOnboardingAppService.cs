using Abp.Application.Services;
using Abp.Dependency;
using ProjectManagement.APIs.ProjectUserOnboarding.Dto;
using System.Threading.Tasks;

namespace ProjectManagement.APIs.ProjectUserOnboarding
{
    public interface IProjectUserOnboardingAppService : IApplicationService, ITransientDependency
    {
        Task<GetOnboardingDto> GetOnboardingInfor(long projectUserId);
        Task<long> OnboardingUser(AddOnboardingDto input);
        Task ForceDone(long projectUserId);
    }
}
