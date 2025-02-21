namespace Trivial.CLI.config;

public record struct IndexConfig(
    Guid Id,
    DateTime LastUpdated,
    string Name,
    string Url,
    List<TemplateIndex> Templates
);

public record struct TemplateIndex(
    Guid RepoId,
    string Id,
    string Name,
    string Key,
    string Path
);