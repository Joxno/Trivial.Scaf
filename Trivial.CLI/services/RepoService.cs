using System.Text.Json;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class RepoService(IRepoRepository Repo, ISettingsService Service) : IRepoService
{
    public Result<IndexConfig> InitRepo(string Path, Maybe<string> Name) => Repo.CreateRepo(Path, Name);

    public Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name) => Try.Invoke(() => {
        if(Url.StartsWith("http", StringComparison.OrdinalIgnoreCase)) ;
        else if(Url.StartsWith("git", StringComparison.OrdinalIgnoreCase) || Url.EndsWith(".git", StringComparison.OrdinalIgnoreCase)) ;
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
        Repo.GetLocalIndexes().FirstOrNone(I => I.Name == Name).ToResult()
            .Bind(Repo.RemoveRemoteIndex)).ToUnit();

    public Maybe<string> GetLocalRemotePathByName(string Name) => Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Name == Name).Map(R => R.Item2);
    public Maybe<string> GetLocalRemotePathById(string Id) => Repo.GetLocalRemotePathById(Guid.Parse(Id));

    public Result<Unit> ConfigureRemoteRepo(string Path, IndexConfig Config) =>
        Repo.GetRepoAtPath(Path).Bind(R => Repo.SaveRemoteIndex(Config with { LastUpdated = DateTime.UtcNow }));

    public Result<IndexConfig> GetRepoAtPath(string Path) => Repo.GetRepoAtPath(Path);
}