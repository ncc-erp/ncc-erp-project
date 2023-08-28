using Abp.Application.Services;
using ProjectManagement.MultiTenancy.Dto;

namespace ProjectManagement.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

