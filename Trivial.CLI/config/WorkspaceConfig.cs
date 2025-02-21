namespace Trivial.CLI.config;

public record struct WorkspaceConfig(
    Guid Id,
    string Name,
    Dictionary<string, object> Data
);