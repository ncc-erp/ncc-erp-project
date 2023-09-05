using System.Threading.Tasks;
using ProjectManagement.Configuration.Dto;

namespace ProjectManagement.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
