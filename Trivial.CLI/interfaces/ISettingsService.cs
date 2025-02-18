using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISettingsService
{
    Result<Unit> Init();
    Result<ReposConfig> GetReposConfig();
    Result<TemplatesConfig> GetTemplatesConfig();
    Result<ScafConfig> GetToolConfig();
    Result<Unit> SaveToolConfig(ScafConfig Config);
}