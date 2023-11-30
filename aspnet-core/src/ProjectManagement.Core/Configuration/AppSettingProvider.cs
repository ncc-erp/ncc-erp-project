using Abp.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace ProjectManagement.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        private static IConfiguration Configuration { get; } = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .Build();

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme,  Configuration["DefaultSettings:UiTheme"], scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.ClientAppId, Configuration["DefaultSettings:ClientAppId"],scopes:SettingScopes.Application| SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.SecurityCode,  Configuration["DefaultSettings:SecurityCode"], scopes:SettingScopes.Application| SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.FinanceUri, Configuration["DefaultSettings:FinanceUri"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.FinanceSecretCode, Configuration["DefaultSettings:FinanceSecretCode"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TimesheetUri, Configuration["DefaultSettings:TimesheetUri"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TimesheetSecretCode, Configuration["DefaultSettings:TimesheetSecretCode"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.AutoUpdateProjectInfoToTimesheetTool, Configuration["DefaultSettings:AutoUpdateProjectInfoToTimesheetTool"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.HRMUri, Configuration["DefaultSettings:HRMUri"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.HRMSecretCode, Configuration["DefaultSettings:HRMSecretCode"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.CanSendDay, Configuration["DefaultSettings:CanSendDay"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.CanSendHour, Configuration["DefaultSettings:CanSendHour"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ExpiredDay, Configuration["DefaultSettings:ExpiredDay"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ExpiredHour, Configuration["DefaultSettings:ExpiredHour"],scopes:SettingScopes.Application|SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.UserBot, Configuration["DefaultSettings:UserBot"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.PasswordBot, Configuration["DefaultSettings:PasswordBot"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuUrl, Configuration["DefaultSettings:KomuUrl"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.NoticeToKomu, Configuration["DefaultSettings:NoticeToKomu"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuSecretCode, Configuration["DefaultSettings:KomuSecretCode"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ProjectUri, Configuration["DefaultSettings:ProjectUri"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuUserNames, Configuration["DefaultSettings:KomuUserNames"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.KomuRoom, Configuration["DefaultSettings:KomuRoom"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.DefaultWorkingHours, Configuration["DefaultSettings:DefaultWorkingHours"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TimeCountDown, Configuration["DefaultSettings:TimeCountDown"],scopes:SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.TrainingRequestChannel,  Configuration["DefaultSettings:TrainingRequestChannel"],scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.MaxCountHistory, Configuration["DefaultSettings:MaxCountHistory"],scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.GuideLine,  Configuration["DefaultSettings:GuideLine"],scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ScoreAudit,  Configuration["DefaultSettings:ScoreAudit"], scopes: SettingScopes.Application | SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.ActiveTimesheetProjectPeriod, Configuration["DefaultSettings:ActiveTimesheetProjectPeriod"]),
                new SettingDefinition(AppSettingNames.InformPm, Configuration["DefaultSettings:InformPm"],scopes: SettingScopes.Application |SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.UpdateChargeStatusBillAccount, Configuration["DefaultSettings:AutoUpdateNotifyBillAccountCharge"],scopes: SettingScopes.Application |SettingScopes.Tenant)
            };
        }
    }
}