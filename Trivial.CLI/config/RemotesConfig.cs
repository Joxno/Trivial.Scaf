namespace Trivial.CLI.config;

public record struct RemotesConfig(
    List<RemoteConfig> Repos
);

public record struct RemoteConfig(
    Guid Id,
    string Url,
    string Name,
    string LocalPath
);