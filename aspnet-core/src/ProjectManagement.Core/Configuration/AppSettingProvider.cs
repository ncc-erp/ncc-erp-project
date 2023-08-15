using Abp.Configuration;
using System.Collections.Generic;

namespace ProjectManagement.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.ClientAppId,"",scopes:SettingScopes.Application| SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.SecurityCode, "", scopes:SettingScopes.Application| SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.FinanceUri,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.FinanceSecretCode,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TimesheetUri,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TimesheetSecretCode,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.AutoUpdateProjectInfoToTimesheetTool,"true",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.HRMUri,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.HRMSecretCode,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.CanSendDay,"",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.CanSendHour,"15",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ExpiredDay,"3",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ExpiredHour,"0",scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.UserBot,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.PasswordBot,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuUrl,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.NoticeToKomu,"true",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuSecretCode,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ProjectUri,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuUserNames,"",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuRoom,"erp-team-ncc",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.DefaultWorkingHours,"8",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TimeCountDown,"180",scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TrainingRequestChannel, "",scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.MaxCountHistory, "12",scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.GuideLine, "{\"Issue\":\"\\u003Cp\\u003Eissue guideline\\u003C/p\\u003E\",\"Risk\":\"B\\u1EA1n c\\u00F3 mu\\u1ED1n\",\"PMNote\":\"\\u003Cp\\u003EPM Note guideline\\u003C/p\\u003E\",\"CriteriaStatus\":\"\\u003Cp\\u003EGuideline Criteria\\u003C/p\\u003E\"} ",scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ScoreAudit, "{\"GIVEN_SCORE\":100,\"PROJECT_SCORE_WHEN_STATUS_GREEN\":85,\"PROJECT_SCORE_WHEN_STATUS_AMBER\":70,\"PROJECT_PROCESS_CRITERIA_RESULT_STATUS_NC\":-20,\"PROJECT_PROCESS_CRITERIA_RESULT_STATUS_OB\":-15,\"PROJECT_PROCESS_CRITERIA_RESULT_STATUS_RE\":15,\"PROJECT_PROCESS_CRITERIA_RESULT_STATUS_EX\":20}", scopes: SettingScopes.Application | SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ActiveTimesheetProjectPeriod,"60000"),
                new SettingDefinition(AppSettingNames.InformPm,"{\r\n    \"channelId\": \"\",\r\n    \"checkDateTimes\": [\r\n      {\r\n        \"isCheck\": false,\r\n        \"time\": \"09:27\",\r\n        \"day\": 5\r\n      }\r\n    ]\r\n  }",scopes: SettingScopes.Application |SettingScopes.Tenant),
            };
        }
    }
}
