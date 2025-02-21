using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IRepoService
{
    Result<IndexConfig> InitRepo(string Path, Maybe<string> Name);
    Result<IndexConfig> IndexRepo(string Path);
    Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name);
    Result<Unit> RemoveRemoteRepo(string Name);
    List<IndexConfig> GetLocalIndexes();
    Maybe<IndexConfig> GetLocalIndexByName(string Name);
    Maybe<IndexConfig> GetLocalIndexById(string Id);
    Maybe<string> GetLocalRemotePathByName(string Name);
    Maybe<string> GetLocalRemotePathById(string Id);
    bool IsTemplateInCacheByName(Guid RemoteId, string TemplateName);
    bool IsTemplateInCacheByKey(Guid RemoteId, string Key);
    bool IsTemplateInCacheById(Guid RemoteId, Guid Id);
    Result<Unit> CacheTemplateFromRemote(Guid TemplateId, Guid RemoteId);
    Maybe<string> GetCachedTemplatePath(Guid RemoteId, Guid TemplateId);
}