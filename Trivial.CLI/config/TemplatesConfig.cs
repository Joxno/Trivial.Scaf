namespace Trivial.CLI.config;

public record struct TemplatesConfig(
    List<string> InstalledTemplatesPaths
);