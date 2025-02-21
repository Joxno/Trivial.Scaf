using System.Text.Json;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.repositories;

public class RepoRepository : IRepoRepository
{
    public Result<IndexConfig> CreateRepo(string Path, Maybe<string> Name) => Try.Invoke(() => {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        if(!Directory.Exists(t_ResolvedPath))
            Directory.CreateDirectory(t_ResolvedPath);

        var t_Repo = new IndexConfig(
            Guid.NewGuid(),
            DateTime.UtcNow,
            "new-repo",
            t_ResolvedPath,
            []
        );

        Name.Then(N => t_Repo = t_Repo with { Name = N });

        var t_RepoIndex = System.IO.Path.Combine(t_ResolvedPath, "repo.scaf.json");
        File.WriteAllText(t_RepoIndex, JsonSerializer.Serialize(t_Repo, new JsonSerializerOptions { WriteIndented = true }));

        return t_Repo;
    });

    public Maybe<IndexConfig> GetLocalIndexById(Guid Id) => GetLocalIndexes().FirstOrNone(I => I.Id == Id);

    public List<IndexConfig> GetLocalIndexes() => Try.Invoke(() => {
        var t_RemotesPath = ScafPaths.GetRemotesPath();
        var t_Remotes = Directory.GetDirectories(t_RemotesPath);
        if(t_Remotes is null || t_Remotes.Length == 0) return [];

        return t_Remotes.Where(R => File.Exists(System.IO.Path.Combine(R, "repo.scaf.json"))).Select(R => {
            var t_RepoIndexPath = System.IO.Path.Combine(R, "repo.scaf.json");

            return JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_RepoIndexPath));
        }).ToList();
    }).ValueOr([]);

    public Maybe<string> GetLocalRemotePathById(Guid Id) => GetLocalRepos().FirstOrNone(R => R.Item1.Id == Id).Map(R => R.Item2);

    public List<(IndexConfig, string)> GetLocalRepos() => Try.Invoke(() => {
        var t_RemotesPath = ScafPaths.GetRemotesPath();
        var t_Remotes = Directory.GetDirectories(t_RemotesPath);
        if(t_Remotes is null || t_Remotes.Length == 0) return [];

        return t_Remotes.Where(R => File.Exists(System.IO.Path.Combine(R, "repo.scaf.json"))).Select(R => {
            var t_RepoIndexPath = System.IO.Path.Combine(R, "repo.scaf.json");

            return (JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_RepoIndexPath)), R);
        }).ToList();
    }).ValueOr([]);

    public Result<IndexConfig> GetRepoAtPath(string Path) => Try.Invoke(() => {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        var t_IndexPath = System.IO.Path.Combine(t_ResolvedPath, "repo.scaf.json");
        if(!File.Exists(t_IndexPath)) return new Exception($"Repo index not found at {t_IndexPath}");

        return JsonSerializer.Deserialize<IndexConfig>(File.ReadAllText(t_IndexPath)).ToResult();
    }).Flatten();

    public Result<Unit> RemoveRemoteIndex(IndexConfig Index) => Try.Invoke(() => {
        var t_Path = System.IO.Path.Combine(ScafPaths.GetRemotesPath(), Index.Id.ToString());
        Directory.Delete(t_Path, true);
    });

    public Result<Unit> SaveRemoteIndex(IndexConfig Index) => Try.Invoke(() => {
        var t_LocalIndexDirPath = System.IO.Path.Combine(ScafPaths.GetRemotesPath(), Index.Id.ToString());
        var t_LocalIndexPath = System.IO.Path.Combine(t_LocalIndexDirPath, $"repo.scaf.json");

        if(!Directory.Exists(t_LocalIndexDirPath))
            Directory.CreateDirectory(t_LocalIndexDirPath);

        File.WriteAllText(t_LocalIndexPath, JsonSerializer.Serialize(Index, new JsonSerializerOptions { WriteIndented = true }));
    });

    public Result<Unit> SaveRemoteIndex(IndexConfig Index, string Path) => Try.Invoke(() => {
        var t_RepoPath = ScafPaths.ResolvePath(Path);
        var t_RepoIndexPath = System.IO.Path.Combine(t_RepoPath, "repo.scaf.json");

        if(!Directory.Exists(t_RepoPath))
            Directory.CreateDirectory(t_RepoPath);

        File.WriteAllText(t_RepoIndexPath, JsonSerializer.Serialize(Index, new JsonSerializerOptions { WriteIndented = true }));
    });
}
