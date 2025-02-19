namespace Trivial.CLI.config;

public record struct RepoIndexConfig(
    Guid Id,
    string Name,
    string Url,
    List<RepoTemplateIndex> Templates
);

public record struct RepoTemplateIndex(
    string Id,
    string Name,
    string Key,
    string Path
);