namespace Trivial.CLI.config;

public record struct ScafConfig(
    TemplatesConfig TemplatesCfg,
    RemoteReposConfig ReposCfg
);