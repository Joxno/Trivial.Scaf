using Trivial.CLI.config;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class SettingsService(ISettingsRepository Repo) : ISettingsService
{
    public Result<Unit> Init() => Repo.Init();
    public Result<RemotesConfig> GetReposConfig() => Repo.GetToolConfig().Map(C => C.ReposCfg);
    public Result<TemplatesConfig> GetTemplatesConfig() => Repo.GetToolConfig().Map(C => C.TemplatesCfg);
    public Result<ToolConfig> GetToolConfig() => Repo.GetToolConfig();
    public Result<Unit> SaveToolConfig(ToolConfig Config) => Repo.SaveToolConfig(Config);
    public Result<Unit> SaveTemplatesConfig(TemplatesConfig Config) => Repo.GetToolConfig().Map(C => C with { TemplatesCfg = Config }).Bind(Repo.SaveToolConfig);
    public Result<Unit> SaveReposConfig(RemotesConfig Config) => Repo.GetToolConfig().Map(C => C with { ReposCfg = Config }).Bind(Repo.SaveToolConfig);

    public Result<Unit> SaveRemoteConfig(RemoteConfig Config) => Try.Invoke(() => {
        GetToolConfig().Then(Cfg => {
            var t_Settings = Cfg with {
                ReposCfg = Cfg.ReposCfg with {
                    Repos = Cfg.ReposCfg.Repos
                        .Where(R => R.Url != Config.Url)
                        .Append(Config).ToList()
                }
            };

            SaveToolConfig(t_Settings);
        });
    });
}