using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class TemplateService(ITemplateRepository Repo) : ITemplateService
{
    public List<Template> GetTemplates() => Repo.GetTemplates();
    public Result<Unit> InstallTemplate(string Path, bool Force) => Repo.InstallTemplate(Path, Force).ToUnit();
    public Result<Unit> InstallScript(string Path) =>  Repo.InstallScript(Path).ToUnit();
    public Result<Unit> UninstallTemplate(string Key) => Repo.DeleteTemplate(Key);
    public Maybe<string> GetTemplatePath(string TemplateKey) => Repo.GetTemplatePath(TemplateKey);
}