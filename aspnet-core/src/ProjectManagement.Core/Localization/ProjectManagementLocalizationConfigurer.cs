using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ProjectManagement.Localization
{
    public static class ProjectManagementLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(ProjectManagementConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(ProjectManagementLocalizationConfigurer).GetAssembly(),
                        "ProjectManagement.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
