using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class UpdateCmd
{
    public static void AddUpdateCmd(this RootCommand Cmd)
    {
        var t_RepoService = Locator.GetIndexService();

        var t_UpdateCmd = Cmd.NewSub("update", "Runs update commands");
        var t_UpdateAllCmd = t_UpdateCmd.NewSub("all", "Updates repos & templates.", () => {
            t_RepoService.UpdateLocalIndexes();
        });
        
        var t_UpdateReposCmd = t_UpdateCmd.NewSub("repos", "Updates local remote indices and templates.", () => {
            t_RepoService.UpdateLocalIndexes();
        });

        var t_UpdateTemplatesCmd = t_UpdateCmd.NewSub("templates", "Updates installed templates.", () => {

        });
    }
}