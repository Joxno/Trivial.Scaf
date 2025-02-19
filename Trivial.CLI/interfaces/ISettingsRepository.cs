using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISettingsRepository
{
    Result<Unit> Init();
    Result<ToolConfig> GetToolConfig();
    Result<Unit> SaveToolConfig(ToolConfig Config);
}