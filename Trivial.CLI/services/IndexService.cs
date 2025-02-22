using System.Text.Json;
using LibGit2Sharp;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class IndexService(IRepoRepository Repo) : IIndexService
{
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

    public Result<Unit> UpdateLocalIndexes() => Try.Invoke(() => {
        var t_Repos = Repo.GetLocalIndexes();
        foreach(var t_Repo in t_Repos)
            UpdateLocalIndex(t_Repo);
    });

    public Result<Unit> UpdateLocalIndex(IndexConfig Index) => Try.Invoke(() => {
        if(Index.Url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            if(Index.Url.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
            {
                _UpdateGitRemoteRepo(Index);
            }
            else if(Index.Url.Contains("github.com", StringComparison.OrdinalIgnoreCase))
            {
                _UpdateGitRemoteRepo(Index);
            }
        }
        else if(Index.Url.StartsWith("git", StringComparison.OrdinalIgnoreCase))
        {
            _UpdateGitRemoteRepo(Index);
        }
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

    private void _UpdateGitRemoteRepo(IndexConfig Index)
    {
        var t_Path = Repo.GetLocalRemotePathById(Index.Id);
        if(!t_Path.HasValue) throw new Exception("Repo not found.");

        Repo.GetRepoAtPath(t_Path.Value).Then(R =>
        {
            using var t_Repo = new Repository(t_Path.Value);
            t_Repo.Reset(ResetMode.Hard, t_Repo.Head.Tip);
            var t_Options = new PullOptions { FetchOptions = new FetchOptions() };
            var t_Signature = new Signature(new Identity("scaf", "scaf"), DateTimeOffset.Now);
            LibGit2Sharp.Commands.Pull(t_Repo, t_Signature, t_Options);
        },
        E => throw E);
    }

    public List<IndexConfig> GetLocalIndexes() => Repo.GetLocalIndexes();

    public Maybe<IndexConfig> GetLocalIndexByName(string Name) =>
        GetLocalIndexes().FirstOrNone(I => I.Name == Name);

    public Maybe<IndexConfig> GetLocalIndexById(string Id) => Repo.GetLocalIndexById(Guid.Parse(Id));
}
