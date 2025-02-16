namespace Trivial.CLI.config;

public record struct ReposConfig(
    List<RepoConfig> Repos
);

public record struct RepoConfig(
    string Url,
    List<string> SearchPaths
);