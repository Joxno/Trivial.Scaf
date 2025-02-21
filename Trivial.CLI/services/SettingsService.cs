using Trivial.CLI.config;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class SettingsService(ISettingsRepository Repo) : ISettingsService
{
    public Result<Unit> Init() => Repo.Init();
    public Result<RemotesConfig> GetReposConfig() => Repo.GetToolConfig().Map(C => C.Repos);
    public Result<TemplatesConfig> GetTemplatesConfig() => Repo.GetToolConfig().Map(C => C.Templates);
    public Result<List<WorkspaceRef>> GetWorkspacesConfig() => Repo.GetToolConfig().Map(C => C.Workspaces);
    public Result<ToolConfig> GetToolConfig() => Repo.GetToolConfig();
    public Result<Unit> SaveToolConfig(ToolConfig Config) => Repo.SaveToolConfig(Config);
    public Result<Unit> SaveTemplatesConfig(TemplatesConfig Config) => Repo.GetToolConfig().Map(C => C with { Templates = Config }).Bind(Repo.SaveToolConfig);
    public Result<Unit> SaveReposConfig(RemotesConfig Config) => Repo.GetToolConfig().Map(C => C with { Repos = Config }).Bind(Repo.SaveToolConfig);

    public Result<Unit> SaveRemoteConfig(RemoteConfig Config) => Try.Invoke(() => {
        GetToolConfig().Then(Cfg => {
            var t_Settings = Cfg with {
                Repos = Cfg.Repos with {
                    Repos = Cfg.Repos.Repos
                        .Where(R => R.Url != Config.Url)
                        .Append(Config).ToList()
                }
            };

            SaveToolConfig(t_Settings);
        });
    });

    public Result<Unit> SaveWorkspacesConfig(WorkspaceRef Config) =>
        GetToolConfig().Bind(Cfg => {
            var t_Settings = Cfg with {
                Workspaces = Cfg.Workspaces
                    .Where(W => W.Id != Config.Id)
                    .Append(Config).ToList()
            };

            return SaveToolConfig(t_Settings);
        });

    public Result<Unit> RemoveWorkspaceConfig(WorkspaceRef Config) =>
        GetToolConfig().Bind(Cfg => {
            var t_Settings = Cfg with {
                Workspaces = Cfg.Workspaces
                    .Where(W => W.Id != Config.Id).ToList()
            };

            return SaveToolConfig(t_Settings);
        });
}