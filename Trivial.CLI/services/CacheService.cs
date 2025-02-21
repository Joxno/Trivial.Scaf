using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class CacheService(IRepoRepository Repo) : ICacheService
{
    public bool IsTemplateInCacheByName(Guid RemoteId, string TemplateName) =>
        FP.Let(Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Id == RemoteId), 
            I => I
            .Bind(I => I.Item1.Templates.FirstOrNone(T => T.Name == TemplateName))
            .Map(T => Path.Combine(I.Value.Item2, T.Path))
            .Map(P => Directory.Exists(P))
        )().ValueOr(false);

    public bool IsTemplateInCacheByKey(Guid RemoteId, string Key) =>
        FP.Let(Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Id == RemoteId), 
            I => I
            .Bind(I => I.Item1.Templates.FirstOrNone(T => T.Key == Key))
            .Map(T => Path.Combine(I.Value.Item2, T.Path))
            .Map(P => Directory.Exists(P))
        )().ValueOr(false);

    public bool IsTemplateInCacheById(Guid RemoteId, Guid Id) =>
        FP.Let(Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Id == RemoteId), 
            I => I
            .Bind(I => I.Item1.Templates.FirstOrNone(T => T.Id == Id.ToString()))
            .Map(T => Path.Combine(I.Value.Item2, T.Path))
            .Map(P => Directory.Exists(P))
        )().ValueOr(false);

    public Result<Unit> CacheTemplateFromRemote(Guid TemplateId, Guid RemoteId)
    {
        var t_RemoteIndex = Repo.GetLocalIndexById(RemoteId);
        if(!t_RemoteIndex.HasValue) return new Exception($"Remote not found with id {RemoteId}");

        var t_TemplateIndex = t_RemoteIndex.Value.Templates.FirstOrNone(T => T.Id == TemplateId.ToString());
        if(!t_TemplateIndex.HasValue) return new Exception($"Template not found with id {TemplateId}");

        var t_Url = t_RemoteIndex.Value.Url;

        if(t_Url.StartsWith("http", StringComparison.OrdinalIgnoreCase)) ;
        else if(t_Url.StartsWith("git", StringComparison.OrdinalIgnoreCase)) ;
        else if(t_Url.StartsWith("ftp", StringComparison.OrdinalIgnoreCase)) ;
        else // File
        {
            return _CacheTemplateFromFileRemote(t_RemoteIndex.Value, t_TemplateIndex.Value);
        }

        return new Exception($"Unsupported remote type for {t_Url}");
    }

    private Result<Unit> _CacheTemplateFromFileRemote(IndexConfig Remote, TemplateIndex TemplateId)
    {
        var t_LocalRemotePath = Repo.GetLocalRemotePathById(Remote.Id);
        if(!t_LocalRemotePath.HasValue) return new Exception($"Local remote path not found for {Remote.Name}");

        var t_RemotePath = Path.Combine(Remote.Url, TemplateId.Path);
        var t_LocalPath = Path.Combine(t_LocalRemotePath.Value, TemplateId.Path);
        
        return Try.Invoke(() => {
            if(Directory.Exists(t_LocalPath))
                Directory.Delete(t_LocalPath, true);

            ScafPaths.CopyTemplate(t_RemotePath, t_LocalPath);
        });
    }

    public Maybe<string> GetCachedTemplatePath(Guid RemoteId, Guid TemplateId)  =>
        FP.Let(Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Id == RemoteId), 
            I => I
            .Bind(I => I.Item1.Templates.FirstOrNone(T => T.Id == TemplateId.ToString()))
            .Map(T => Path.Combine(I.Value.Item2, T.Path))
        )();
}
