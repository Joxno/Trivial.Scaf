namespace Trivial.CLI.config;

public record struct ToolConfig(
    TemplatesConfig Templates,
    RemotesConfig Repos,
    List<WorkspaceRef> Workspaces
);

public record struct WorkspaceRef(
    Guid Id,
    string Name,
    string Path
);