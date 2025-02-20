using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class WorkspaceCmd
{
    public static void AddWorkspaceCmd(this RootCommand Cmd)
    {
        var t_WorkspaceService = Locator.GetWorkspaceService();
        var t_WorkspaceCmd = Cmd.NewSub("workspace", "Workspace commands");

        t_WorkspaceCmd.NewSub("add", "Adds a workspace", (Path) => {
            var t_ResolvedPath = ScafPaths.ResolvePath(Path);
            t_WorkspaceService.AddWorkspace(t_ResolvedPath)
                .Then(
                    _ => Console.WriteLine("Workspace Added."),
                    E => Console.WriteLine(E.Message)
                );
        },
        new Argument<string>("path", () => "./", "The path of the workspace")
        );
    }
}