using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISettingsService
{
    Result<Unit> Init();
    Result<RemotesConfig> GetReposConfig();
    Result<TemplatesConfig> GetTemplatesConfig();
    Result<List<WorkspaceRef>> GetWorkspacesConfig();
    Result<ToolConfig> GetToolConfig();
    Result<Unit> SaveToolConfig(ToolConfig Config);
    Result<Unit> SaveTemplatesConfig(TemplatesConfig Config);
    Result<Unit> SaveReposConfig(RemotesConfig Config);
    Result<Unit> SaveRemoteConfig(RemoteConfig Config);
    Result<Unit> SaveWorkspacesConfig(WorkspaceRef Config);
    Result<Unit> RemoveWorkspaceConfig(WorkspaceRef Config);
}