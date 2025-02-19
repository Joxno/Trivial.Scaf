using Trivial.CLI.config;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class SettingsService(ISettingsRepository Repo) : ISettingsService
{
    public Result<Unit> Init() => Repo.Init();
    public Result<RemoteReposConfig> GetReposConfig() => Repo.GetToolConfig().Map(C => C.ReposCfg);
    public Result<TemplatesConfig> GetTemplatesConfig() => Repo.GetToolConfig().Map(C => C.TemplatesCfg);
    public Result<ScafConfig> GetToolConfig() => Repo.GetToolConfig();
    public Result<Unit> SaveToolConfig(ScafConfig Config) => Repo.SaveToolConfig(Config);
    public Result<Unit> SaveTemplatesConfig(TemplatesConfig Config) => Repo.GetToolConfig().Map(C => C with { TemplatesCfg = Config }).Bind(Repo.SaveToolConfig);
    public Result<Unit> SaveReposConfig(RemoteReposConfig Config) => Repo.GetToolConfig().Map(C => C with { ReposCfg = Config }).Bind(Repo.SaveToolConfig);
}