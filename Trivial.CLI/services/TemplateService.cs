using System.Text.Json;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class TemplateService : ITemplateService
{
    private ITemplateRepository m_Repo;

    public TemplateService(ITemplateRepository Repo)
    {
        m_Repo = Repo;
    }

    public List<Template> GetTemplates() =>
        m_Repo.GetTemplates();

    public Result<Unit> InstallTemplate(string Path, bool Force) => 
        m_Repo.InstallTemplate(Path, Force).ToUnit();

    public Result<Unit> InstallScript(string Path) => 
        m_Repo.InstallScript(Path).ToUnit();

    public Result<Unit> UninstallTemplate(string Key) => 
        m_Repo.DeleteTemplate(Key);

    public Maybe<string> GetTemplatePath(string TemplateKey) =>
        m_Repo.GetTemplatePath(TemplateKey);
}