using System.Text.Json;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class RepoService(ISettingsService Service) : IRepoService
{
    public Result<IndexConfig> InitRepo(string Path, Maybe<string> Name) => Try.Invoke(() => {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        if(!Directory.Exists(t_ResolvedPath))
            Directory.CreateDirectory(t_ResolvedPath);

        var t_Repo = new IndexConfig(
            Guid.NewGuid(),
            "new-repo",
            t_ResolvedPath,
            []
        );

        Name.Then(N => t_Repo = t_Repo with { Name = N });

        var t_RepoIndex = System.IO.Path.Combine(t_ResolvedPath, "repo.scaf.json");
        File.WriteAllText(t_RepoIndex, JsonSerializer.Serialize(t_Repo, new JsonSerializerOptions { WriteIndented = true }));

        return t_Repo;
    });

    public Result<IndexConfig> IndexRepo(string Path) => Try.Invoke(() => {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        var t_RepoIndexPath = System.IO.Path.Combine(t_ResolvedPath, "repo.scaf.json");
        if(!File.Exists(t_RepoIndexPath))
            return new Exception($"Repo index not found at {t_RepoIndexPath}");

        var t_Repo = JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_RepoIndexPath));
        var t_IndexedTemplates = _IndexTemplates(t_ResolvedPath).ToList();
        t_Repo = t_Repo with { 
            Templates = t_IndexedTemplates.Select(I => I with { Path = I.Path.Replace(t_ResolvedPath, "") }).ToList()
        };

        File.WriteAllText(t_RepoIndexPath, JsonSerializer.Serialize(t_Repo, new JsonSerializerOptions { WriteIndented = true }));

        return t_Repo.ToResult();
    }).Flatten();

    private List<TemplateIndex> _IndexTemplates(string Path)
    {
        var t_Index = new List<TemplateIndex>();
        var t_Dirs = Directory.GetDirectories(Path);
        if(t_Dirs is null || t_Dirs.Length == 0) return [];

        foreach(var t_Dir in t_Dirs)
        {
            var t_TemplateIndex = System.IO.Path.Combine(t_Dir, "template.scaf.json");
            if(!File.Exists(t_TemplateIndex))
            {
                t_Index.AddRange(_IndexTemplates(t_Dir));
                continue;
            }

            Console.WriteLine($"Found Template: {t_TemplateIndex}");

            Try.Invoke(() => {
                var t_Template = JsonSerializer.Deserialize<Template>(File.ReadAllText(t_TemplateIndex));
                t_Index.Add(new(t_Template.Id.ToString(), t_Template.Name, t_Template.Key, t_Dir.ToString()));
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

    private void _AddFileRemoteRepo(string Path, Maybe<string> Name)
    {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        var t_IndexPath = System.IO.Path.Combine(t_ResolvedPath, "repo.scaf.json");
        if(!File.Exists(t_IndexPath)) throw new Exception($"Repo index not found at {t_IndexPath}");

        var t_Repo = JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_IndexPath));
        var t_LocalIndexDirPath = System.IO.Path.Combine(ScafPaths.GetRemotesPath(), t_Repo.Id.ToString());
        var t_LocalIndexPath = System.IO.Path.Combine(t_LocalIndexDirPath, $"repo.scaf.json");

        if(!Directory.Exists(t_LocalIndexDirPath))
            Directory.CreateDirectory(t_LocalIndexDirPath);

        Name.Then(N => t_Repo = t_Repo with { Name = N });
        Service.SaveRemoteConfig(new(t_Repo.Url, t_Repo.Name));

        File.WriteAllText(t_LocalIndexPath, JsonSerializer.Serialize(t_Repo, new JsonSerializerOptions { WriteIndented = true }));
    }

    public Result<Unit> RemoveRemoteRepo(string Name) => Try.Invoke(() =>
        GetLocalIndexes().FirstOrNone(I => I.Name == Name).ToResult()
            .Then(I => {
                var t_Path = System.IO.Path.Combine(ScafPaths.GetRemotesPath(), I.Id.ToString());
                Directory.Delete(t_Path, true);
                
            }));
        

    public List<IndexConfig> GetLocalIndexes() => Try.Invoke(() => {
        var t_RemotesPath = ScafPaths.GetRemotesPath();
        var t_Remotes = Directory.GetDirectories(t_RemotesPath);
        if(t_Remotes is null || t_Remotes.Length == 0) return [];

        return t_Remotes.Where(R => File.Exists(System.IO.Path.Combine(R, "repo.scaf.json"))).Select(R => {
            var t_RepoIndexPath = System.IO.Path.Combine(R, "repo.scaf.json");

            return JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_RepoIndexPath));
        }).ToList();
    }).ValueOr([]);
}