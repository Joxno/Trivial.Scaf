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
        var t_IndexedTemplates = _IndexTemplates(t_ResolvedPath).ToList();
        t_Repo = t_Repo with { 
            LastUpdated = DateTime.UtcNow,
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

    private void _AddFileRemoteRepo(string Path, Maybe<string> Name) => Repo.GetRepoAtPath(Path).Then(R =>
    {
        Name.Then(N => R = R with { Name = N });
        Repo.SaveRemoteIndex(R);
        Service.SaveRemoteConfig(new(R.Id, R.Url, R.Name));
    });

    public Result<Unit> RemoveRemoteRepo(string Name) => Try.Invoke(() =>
        GetLocalIndexes().FirstOrNone(I => I.Name == Name).ToResult()
            .Bind(Repo.RemoveRemoteIndex)).ToUnit();
        

    public List<IndexConfig> GetLocalIndexes() => Repo.GetLocalIndexes();

    public Maybe<IndexConfig> GetIndexByName(string Name) =>
        GetLocalIndexes().FirstOrNone(I => I.Name == Name);

    public Maybe<IndexConfig> GetIndexById(string Id) =>
        GetLocalIndexes().FirstOrNone(I => I.Id.ToString() == Id);
}