using Trivial.CLI.config;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class SearchService(IRepoService Service) : ISearchService
{
    public List<TemplateIndex> SearchForTemplateById(Guid Id) => 
        Service.GetLocalIndexes().SelectMany(R => R.Templates).Where(T => T.Id == Id.ToString()).ToList();

    public List<TemplateIndex> SearchForTemplateByKey(string Key) =>
        Service.GetLocalIndexes().SelectMany(R => R.Templates).Where(T => T.Key == Key).ToList();

    public List<TemplateIndex> SearchForTemplateByName(string Name) =>
        Service.GetLocalIndexes().SelectMany(R => R.Templates).Where(T => T.Name == Name).ToList();
}
