using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.repositories;

namespace Trivial.CLI.services;

public static class Locator
{
    private static ITemplateRepository m_TemplateRepository = new TemplateRepository([ScafPaths.GetTemplatesPath()]);
    private static ITemplateService m_TemplateService = new TemplateService(m_TemplateRepository);

    public static ITemplateService GetTemplateService() => m_TemplateService;
}