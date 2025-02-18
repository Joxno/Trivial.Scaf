using Trivial.CLI.config;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class SettingsService(ISettingsRepository Repo) : ISettingsService
{
    public Result<Unit> Init() => Repo.Init();
    public Result<ReposConfig> GetReposConfig() => Repo.GetToolConfig().Map(C => C.ReposCfg);
    public Result<TemplatesConfig> GetTemplatesConfig() => Repo.GetToolConfig().Map(C => C.TemplatesCfg);
    public Result<ScafConfig> GetToolConfig() => Repo.GetToolConfig();
    public Result<Unit> SaveToolConfig(ScafConfig Config) => Repo.SaveToolConfig(Config);
}