using System.Text.Json;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.repositories;

public class SettingsRepository : ISettingsRepository
{
    private const string m_ConfigFileName = "config.scaf.json";

    public Result<Unit> Init() => Try.Invoke(() => {
        if(File.Exists(Path.Combine(ScafPaths.GetConfigPath(), m_ConfigFileName))) return Result.Unit;

        var t_DefaultConfig = new ToolConfig(
            new TemplatesConfig([ScafPaths.GetTemplatesPath()]),
            new RemotesConfig([])
        );

        return SaveToolConfig(t_DefaultConfig);
    }).Flatten();

    public Result<ToolConfig> GetToolConfig() => Try.Invoke(() => {
        var t_ConfigPath = Path.Combine(ScafPaths.GetConfigPath(), m_ConfigFileName);
        var t_Json = File.ReadAllText(t_ConfigPath);
        return JsonSerializer.Deserialize<ToolConfig>(t_Json);
    });

    public Result<Unit> SaveToolConfig(ToolConfig Config) => Try.Invoke(() => {
        var t_ConfigPath = Path.Combine(ScafPaths.GetConfigPath(), m_ConfigFileName);
        var t_Json = JsonSerializer.Serialize(Config);
        File.WriteAllText(t_ConfigPath, t_Json);
    });
}
