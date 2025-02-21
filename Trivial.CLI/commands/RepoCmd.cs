using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class RepoCmd
{
    public static void AddRepoCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetRepoService();
        var t_RepoCmd = Cmd.NewSub("repo", "Repo commands");
        t_RepoCmd.NewSub("add", "Adds a remote repo", (Url, Name) => {
            t_Service.AddRemoteRepo(Url, Name!.ToMaybe())
                .Then(
                    _ => Console.WriteLine("Remote Repo Added."),
                    E => Console.WriteLine(E.Message)
                );
        }, 
        new Argument<string>("url", () => "./", "The url of the remote repo"),
        new Option<string?>(["--name", "-n"], () => null, "The name of the remote repo")
        );

        t_RepoCmd.NewSub("remove", "Removes a remote repo", Name => {
            t_Service.RemoveRemoteRepo(Name)
                .Then(
                    _ => Console.WriteLine("Remote Repo Removed."),
                    E => Console.WriteLine(E.Message)
                );
        }, new Argument<string>("name", "The name of the remote repo"));

        t_RepoCmd.NewSub("configure", "Configures a repo", (Path, Name, Url) => {
            var t_ResolvedPath = ScafPaths.ResolvePath(Path);
            t_Service.GetRepoAtPath(t_ResolvedPath)
                .Bind(R => t_Service.ConfigureRemoteRepo(t_ResolvedPath, R with {
                    Name = Name ?? R.Name,
                    Url = Url ?? R.Url
                }));
        },
        new Argument<string>("path", () => "./", "The path to the repo"),
        new Option<string?>(["--name", "-n"], () => null, "The name of the repo"),
        new Option<string?>(["--url", "-u"], () => null, "The url of the repo")
        );
    }
}