namespace Trivial.CLI.config;

public record struct RemoteReposConfig(
    List<RemoteRepoConfig> Repos
);

public record struct RemoteRepoConfig(
    string Url,
    string Name
);