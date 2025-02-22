using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.repositories;

namespace Trivial.CLI.services;

public static class Locator
{
    private static ISettingsRepository m_SettingsRepository = new SettingsRepository();
    private static ISettingsService m_SettingsService = new SettingsService(m_SettingsRepository);
    private static ITemplateRepository m_TemplateRepository = new TemplateRepository(
        m_SettingsService.GetTemplatesConfig().Map(C => C.InstalledTemplatesPaths).ValueOr([ScafPaths.GetTemplatesPath()])
    );
    private static IRepoRepository m_RepoRepository = new RepoRepository();
    private static IRepoService m_RepoService = new RepoService(m_RepoRepository, m_SettingsService);
    private static IIndexService m_IndexService = new IndexService(m_RepoRepository);
    private static ISearchService m_RepoSearchService = new SearchService(m_IndexService);
    private static IWorkspaceService m_WorkspaceService = new WorkspaceService(m_SettingsService);
    private static ICacheService m_CacheService = new CacheService(m_RepoRepository);
    private static ITemplateService m_TemplateService = new TemplateService(m_TemplateRepository, m_RepoSearchService, m_CacheService, m_IndexService);
    private static IContextService m_ContextService = new ContextService(m_WorkspaceService);

    public static ITemplateService GetTemplateService() => m_TemplateService;
    public static ISettingsService GetSettingsService() => m_SettingsService;
    public static IRepoService GetRepoService() => m_RepoService;
    public static ISearchService GetSearchService() => m_RepoSearchService;
    public static IWorkspaceService GetWorkspaceService() => m_WorkspaceService;
    public static IContextService GetContextService() => m_ContextService;
    public static ICacheService GetCacheService() => m_CacheService;
    public static IIndexService GetIndexService() => m_IndexService;
}