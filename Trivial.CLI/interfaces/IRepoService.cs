using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IRepoService
{
    Result<IndexConfig> InitRepo(string Path, Maybe<string> Name);
    Result<IndexConfig> IndexRepo(string Path);
    Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name);
    Result<Unit> RemoveRemoteRepo(string Name);
    List<IndexConfig> GetLocalIndexes();
}