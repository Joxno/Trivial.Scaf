using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface ISearchService
{
    List<TemplateIndex> SearchForTemplateById(Guid Id);
    List<TemplateIndex> SearchForTemplateByKey(string Key);
    List<TemplateIndex> SearchForTemplateByName(string Name);
}