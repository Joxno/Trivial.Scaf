using Trivial.CLI.config;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class SearchService(IRepoService Service) : ISearchService
{
    public List<RepoTemplateIndex> SearchForTemplateById(Guid Id) => 
        Service.GetIndexes().SelectMany(R => R.Templates).Where(T => T.Id == Id.ToString()).ToList();

    public List<RepoTemplateIndex> SearchForTemplateByKey(string Key) =>
        Service.GetIndexes().SelectMany(R => R.Templates).Where(T => T.Key == Key).ToList();

    public List<RepoTemplateIndex> SearchForTemplateByName(string Name) =>
        Service.GetIndexes().SelectMany(R => R.Templates).Where(T => T.Name == Name).ToList();
}
