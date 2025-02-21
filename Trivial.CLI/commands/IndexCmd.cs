using System.CommandLine;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class IndexCmd
{
    public static void AddIndexCmd(this RootCommand RootCmd)
    {
        var t_Service = Locator.GetIndexService();

        var t_IndexCmd = RootCmd.NewSub("index", "Indexing commands");
        var t_IndexRepoCmd = t_IndexCmd.NewSub("repo", "Indexes a repo", Path => {
            t_Service.IndexRepo(Path)
                .Then(
                    _ => Console.WriteLine("Repo Indexed."),
                    E => Console.WriteLine(E.Message)
                );
        }, new Argument<string>("path", () => "./", "The path to the repo"));
    }
}