using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISearchService
{
    List<RepoTemplateIndex> SearchForTemplateById(Guid Id);
    List<RepoTemplateIndex> SearchForTemplateByKey(string Key);
    List<RepoTemplateIndex> SearchForTemplateByName(string Name);
}