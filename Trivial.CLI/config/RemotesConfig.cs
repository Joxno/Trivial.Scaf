namespace Trivial.CLI.config;

public record struct RemotesConfig(
    List<RemoteConfig> Repos
);

public record struct RemoteConfig(
    string Url,
    string Name
);