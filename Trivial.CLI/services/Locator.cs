using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.repositories;

namespace Trivial.CLI.services;

public static class Locator
{
    private static ISettingsRepository m_SettingsRepository = new SettingsRepository();
    private static ISettingsService m_SettingsService = new SettingsService(m_SettingsRepository);
    private static ITemplateRepository m_TemplateRepository = new TemplateRepository(m_SettingsService.GetTemplatesConfig().Map(C => C.InstalledTemplatesPaths).ValueOr([ScafPaths.GetTemplatesPath()]));
    private static ITemplateService m_TemplateService = new TemplateService(m_TemplateRepository);

    public static ITemplateService GetTemplateService() => m_TemplateService;
    public static ISettingsService GetSettingsService() => m_SettingsService;
}