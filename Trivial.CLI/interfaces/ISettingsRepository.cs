using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISettingsRepository
{
    Result<Unit> Init();
    Result<ScafConfig> GetToolConfig();
    Result<Unit> SaveToolConfig(ScafConfig Config);
}