using System.Text.Json;
using LibGit2Sharp;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class RepoService(IRepoRepository Repo, ISettingsService Service) : IRepoService
{
    public Result<IndexConfig> InitRepo(string Path, Maybe<string> Name) => Repo.CreateRepo(Path, Name);

    public Result<Unit> AddRemoteRepo(string Url, Maybe<string> Name) => Try.Invoke(() => {
        if(Url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
        {
            if(Url.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
            {
                _AddGitRemoteRepo(Url, Name);
            }
            else if(Url.Contains("github.com", StringComparison.OrdinalIgnoreCase))
            {
                _AddGitRemoteRepo(Url, Name);
            }
        }
        else if(Url.StartsWith("git", StringComparison.OrdinalIgnoreCase) || Url.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
        {
            _AddGitRemoteRepo(Url, Name);
        }
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

    private void _AddGitRemoteRepo(string Url, Maybe<string> Name)
    {
        var t_UniqueName = Guid.NewGuid().ToString();
        var t_Path = Path.Combine(ScafPaths.GetRemotesPath(), t_UniqueName);

        Repository.Clone(Url, t_Path);
        Repo.GetRepoAtPath(t_Path).Then(R =>
        {
            Name.Then(N => R = R with { Name = N });
            Service.SaveRemoteConfig(new(R.Id, R.Url, R.Name, t_Path));
        },
        E => {
            Directory.Delete(t_Path, true);
            throw E;
        });
    }

    public Result<Unit> RemoveRemoteRepo(string Name) => Try.Invoke(() =>
        Repo.GetLocalIndexes().FirstOrNone(I => I.Name == Name).ToResult()
            .Bind(Repo.RemoveRemoteIndex)).ToUnit();

    public Maybe<string> GetLocalRemotePathByName(string Name) => Repo.GetLocalRepos().FirstOrNone(R => R.Item1.Name == Name).Map(R => R.Item2);
    public Maybe<string> GetLocalRemotePathById(string Id) => Repo.GetLocalRemotePathById(Guid.Parse(Id));

    public Result<Unit> ConfigureRemoteRepo(string Path, IndexConfig Config) =>
        Repo.GetRepoAtPath(Path).Bind(R => Repo.SaveRemoteIndex(Config with { LastUpdated = DateTime.UtcNow }, Path));

    public Result<IndexConfig> GetRepoAtPath(string Path) => Repo.GetRepoAtPath(Path);
}