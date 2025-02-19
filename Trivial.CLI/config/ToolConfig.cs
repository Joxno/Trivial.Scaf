namespace Trivial.CLI.config;

public record struct ToolConfig(
    TemplatesConfig TemplatesCfg,
    RemotesConfig ReposCfg
);