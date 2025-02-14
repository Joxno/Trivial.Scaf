namespace Trivial.CLI.models;

public record struct Template(
    string Name,
    string Key,
    string Description,
    List<TemplateRun> Triggers
);

public record struct TemplateRun(
    string Keyword,
    string Description,
    List<TemplateRunArg> Parameters,
    List<TemplateRunParam> Options,
    TemplateRunAction Action
);

public record struct TemplateRunArg(
    string Name,
    string Description
);

public record struct TemplateRunParam(
    List<string> Names,
    string Description,
    string MapToName,
    string DefaultValue
);

public record struct TemplateRunAction(
    string Executable
);