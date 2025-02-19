using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IRepoService
{
    Result<RepoIndexConfig> InitRepo(string Path, Maybe<string> Name);
    Result<RepoIndexConfig> IndexRepo(string Path);
    Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name);
    Result<Unit> RemoveRemoteRepo(string Name);
    List<RepoIndexConfig> GetIndexes();
}