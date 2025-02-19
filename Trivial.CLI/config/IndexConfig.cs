namespace Trivial.CLI.config;

public record struct IndexConfig(
    Guid Id,
    string Name,
    string Url,
    List<TemplateIndex> Templates
);

public record struct TemplateIndex(
    string Id,
    string Name,
    string Key,
    string Path
);