using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IRepoService
{
    Result<IndexConfig> InitRepo(string Path, Maybe<string> Name);
    Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name);
    Result<Unit> RemoveRemoteRepo(string Name);
    Result<Unit> ConfigureRemoteRepo(string Path, IndexConfig Config);
    Result<IndexConfig> GetRepoAtPath(string Path);
    Maybe<string> GetLocalRemotePathByName(string Name);
    Maybe<string> GetLocalRemotePathById(string Id);
}