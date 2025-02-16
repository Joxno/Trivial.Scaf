using Trivial.CLI.models;

namespace Trivial.CLI.interfaces;

public interface ITemplateRepository
{
    List<Template> GetTemplates();
    Maybe<Template> GetTemplate(string Key);
    Result<Unit> DeleteTemplate(Template Template);
    Result<Unit> DeleteTemplate(string Key);
    Maybe<string> GetTemplatePath(Template Template);
    Maybe<string> GetTemplatePath(string Key);

    Result<Template> InstallTemplate(string Path, bool Force);
    Result<Template> InstallScript(string Path);
}