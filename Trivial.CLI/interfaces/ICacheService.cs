namespace Trivial.CLI.interfaces;

public interface ICacheService
{
    bool IsTemplateInCacheByName(Guid RemoteId, string TemplateName);
    bool IsTemplateInCacheByKey(Guid RemoteId, string Key);
    bool IsTemplateInCacheById(Guid RemoteId, Guid Id);
    Result<Unit> CacheTemplateFromRemote(Guid TemplateId, Guid RemoteId);
    Maybe<string> GetCachedTemplatePath(Guid RemoteId, Guid TemplateId);
}