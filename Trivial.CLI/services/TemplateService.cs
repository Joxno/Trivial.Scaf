using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class TemplateService(ITemplateRepository Repo, ISearchService SearchService, ICacheService CacheService, IIndexService IndexService) : ITemplateService
{
    public List<Template> GetTemplates() => Repo.GetTemplates();
    public Result<Unit> InstallTemplate(string Path, bool Force) => Repo.InstallTemplate(Path, Force).ToUnit();
    public Result<Unit> InstallScript(string Path) =>  Repo.InstallScript(Path).ToUnit();
    public Result<Unit> UninstallTemplate(string Key) => Repo.DeleteTemplate(Key);
    public Maybe<string> GetTemplatePath(string TemplateKey) => Repo.GetTemplatePath(TemplateKey);

    public Result<Unit> InstallTemplateFromRepo(string Name, bool Force)
    {
        var t_SearchResults = SearchService.SearchForTemplateByName(Name);
        if(t_SearchResults.Count == 0) return new Exception($"No template found with the name {Name}");
        if(t_SearchResults.Count > 1) return new Exception($"Multiple templates found with the name {Name}");

        var t_Template = t_SearchResults[0];
        var t_Repo = IndexService.GetLocalIndexes().FirstOrNone(I => I.Templates.Any(T => T.Id == t_Template.Id));
        if(!t_Repo.HasValue) return new Exception($"Repo not found for template {Name}");

        if(!CacheService.IsTemplateInCacheById(t_Repo.Value.Id, Guid.Parse(t_Template.Id)))
        {
            var t_Result = CacheService.CacheTemplateFromRemote(Guid.Parse(t_Template.Id), t_Repo.Value.Id);
            if(t_Result.HasError) return t_Result;
        }

        var t_Path = CacheService.GetCachedTemplatePath(t_Repo.Value.Id, Guid.Parse(t_Template.Id));
        if(!t_Path.HasValue) return new Exception($"Template not found in cache for {Name}");

        return Repo.InstallTemplate(t_Path.Value, Force).ToUnit();
    }
}