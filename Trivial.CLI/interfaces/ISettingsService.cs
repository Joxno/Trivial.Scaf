using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISettingsService
{
    Result<Unit> Init();
    Result<RemoteReposConfig> GetReposConfig();
    Result<TemplatesConfig> GetTemplatesConfig();
    Result<ScafConfig> GetToolConfig();
    Result<Unit> SaveToolConfig(ScafConfig Config);
    Result<Unit> SaveTemplatesConfig(TemplatesConfig Config);
    Result<Unit> SaveReposConfig(RemoteReposConfig Config);
}