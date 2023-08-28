using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ProjectManagement.Roles.Dto;
using ProjectManagement.Users.Dto;

namespace ProjectManagement.Users
{
    public interface IUserAppService : IAsyncCrudAppService<UserDto, long, PagedUserResultRequestDto, CreateUserDto, UserDto>
    {
        Task<ListResultDto<RoleDto>> GetRoles();

        Task ChangeLanguage(ChangeUserLanguageDto input);

        Task<bool> ChangePassword(ChangePasswordDto input);
        Task<EmployeeInformationDto> GetEmployeeInformation(string email);
        Task<long?> UpdateKomuId(long userId);
    }
}
