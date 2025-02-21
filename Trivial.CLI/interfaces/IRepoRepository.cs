using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IRepoRepository
{
    Result<IndexConfig> CreateRepo(string Path, Maybe<string> Name);
    Result<IndexConfig> GetRepoAtPath(string Path);
    List<(IndexConfig, string)> GetLocalRepos();
    Result<Unit> SaveRemoteIndex(IndexConfig Index);
    Result<Unit> RemoveRemoteIndex(IndexConfig Index);
    List<IndexConfig> GetLocalIndexes();
    Maybe<string> GetLocalRemotePathById(Guid Id);
    Maybe<IndexConfig> GetLocalIndexById(Guid Id);
}