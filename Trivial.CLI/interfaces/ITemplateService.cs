using Trivial.CLI.models;

namespace Trivial.CLI.interfaces;

public interface ITemplateService
{
    Result<Unit> InstallTemplate(string Path, bool Force);
    Result<Unit> InstallScript(string Path);
    Result<Unit> UninstallTemplate(string Key);
    Result<Unit> InstallTemplateFromRepo(string Name, bool Force);
    List<Template> GetTemplates();
    Maybe<string> GetTemplatePath(string TemplateName);
}