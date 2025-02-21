using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IIndexService
{
    Result<IndexConfig> IndexRepo(string Path);
    Result<Unit> UpdateLocalIndexes();
    Result<Unit> UpdateLocalIndex(IndexConfig Index);
    List<IndexConfig> GetLocalIndexes();
    Maybe<IndexConfig> GetLocalIndexByName(string Name);
    Maybe<IndexConfig> GetLocalIndexById(string Id);
}