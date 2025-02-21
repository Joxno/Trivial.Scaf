using System.Text.Json;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class RepoService(IRepoRepository Repo, ISettingsService Service) : IRepoService
{
    public Result<IndexConfig> InitRepo(string Path, Maybe<string> Name) => Repo.CreateRepo(Path, Name);

    public Result<IndexConfig> IndexRepo(string Path) => Try.Invoke(() => {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        var t_RepoIndexPath = System.IO.Path.Combine(t_ResolvedPath, "repo.scaf.json");
        if(!File.Exists(t_RepoIndexPath))
            return new Exception($"Repo index not found at {t_RepoIndexPath}");

        var t_Repo = JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_RepoIndexPath));
        var t_IndexedTemplates = _IndexTemplates(t_Repo, t_ResolvedPath).ToList();
        t_Repo = t_Repo with { 
            LastUpdated = DateTime.UtcNow,
            Templates = t_IndexedTemplates.Select(I => I with { Path = I.Path.Replace(t_ResolvedPath, "") }).ToList()
        };

        File.WriteAllText(t_RepoIndexPath, JsonSerializer.Serialize(t_Repo, new JsonSerializerOptions { WriteIndented = true }));

        return t_Repo.ToResult();
    }).Flatten();

    private List<TemplateIndex> _IndexTemplates(IndexConfig Repo, string Path)
    {
        var t_Index = new List<TemplateIndex>();
        var t_Dirs = Directory.GetDirectories(Path);
        if(t_Dirs is null || t_Dirs.Length == 0) return [];

        foreach(var t_Dir in t_Dirs)
        {
            var t_TemplateIndex = System.IO.Path.Combine(t_Dir, "template.scaf.json");
            if(!File.Exists(t_TemplateIndex))
            {
                t_Index.AddRange(_IndexTemplates(Repo, t_Dir));
                continue;
            }

            Console.WriteLine($"Found Template: {t_TemplateIndex}");

            Try.Invoke(() => {
                var t_Template = JsonSerializer.Deserialize<Template>(File.ReadAllText(t_TemplateIndex));
                t_Index.Add(new(Repo.Id, t_Template.Id.ToString(), t_Template.Name, t_Template.Key, t_Dir.ToString()));
            });
        }

        return t_Index;
    }

    public Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name) => Try.Invoke(() => {
        if(Url.StartsWith("http", StringComparison.OrdinalIgnoreCase)) ;
        else if(Url.StartsWith("git", StringComparison.OrdinalIgnoreCase)) ;
        else if(Url.StartsWith("ftp", StringComparison.OrdinalIgnoreCase)) ;
        else // File
        {
            _AddFileRemoteRepo(Url, Name);
        }
    });

    private void _AddFileRemoteRepo(string Path, Maybe<string> Name) => Repo.GetRepoAtPath(Path).Then(R =>
    {
        Name.Then(N => R = R with { Name = N });
        Repo.SaveRemoteIndex(R);
        Service.SaveRemoteConfig(new(R.Id, R.Url, R.Name, ScafPaths.ResolvePath(Path)));
    });

    public Result<Unit> RemoveRemoteRepo(string Name) => Try.Invoke(() =>
        GetLocalIndexes().FirstOrNone(I => I.Name == Name).ToResult()
            .Bind(Repo.RemoveRemoteIndex)).ToUnit();
        
    public Result<Unit> UpdateLocalIndexes() => Try.Invoke(() => {
        var t_Repos = Repo.GetLocalIndexes();
        foreach(var t_Repo in t_Repos)
            UpdateLocalIndex(t_Repo);
    });

    public Result<Unit> UpdateLocalIndex(IndexConfig Index) => Try.Invoke(() => {
        if(Index.Url.StartsWith("http", StringComparison.OrdinalIgnoreCase)) ;
        else if(Index.Url.StartsWith("git", StringComparison.OrdinalIgnoreCase)) ;
        else if(Index.Url.StartsWith("ftp", StringComparison.OrdinalIgnoreCase)) ;
        else // File
        {
            _UpdateFileRepo(Index.Url, Index.Name);
        }
    });

    private void _UpdateFileRepo(string Path, string Name) => Repo.GetRepoAtPath(Path).Then(R =>
    {
        R = R with { Name = Name };
        Repo.SaveRemoteIndex(R);
    });

    public List<IndexConfig> GetLocalIndexes() => Repo.GetLocalIndexes();

    public Maybe<IndexConfig> GetLocalIndexByName(string Name) =>
        GetLocalIndexes().FirstOrNone(I => I.Name == Name);

    public Maybe<IndexConfig> GetLocalIndexById(string Id) =>
        GetLocalIndexes().FirstOrNone(I => I.Id.ToString() == Id);

    public Maybe<string> GetLocalRemotePathByName(string Name) => Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Name == Name).Map(R => R.Item2);
    public Maybe<string> GetLocalRemotePathById(string Id) => Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Id.ToString() == Id).Map(R => R.Item2);

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
        var t_RemoteIndex = GetLocalIndexById(RemoteId.ToString());
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
        var t_LocalRemotePath = GetLocalRemotePathById(Remote.Id.ToString());
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