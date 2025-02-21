using System.CommandLine;
using System.IO;
using System.Text.Json.Nodes;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;
using Trivial.Utilities;

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

        t_WorkspaceCmd.NewSub("list", "Lists closest workspace", (Path) => {
                var t_ResolvedPath = ScafPaths.ResolvePath(Path);
                t_WorkspaceService.FindWorkspaceFromPath(t_ResolvedPath)
                    .ThenOrElse(
                        W => Console.WriteLine(W.Workspace.ToJson()),
                        () => Console.WriteLine("No workspace found.")
                    );
            },
            new Argument<string>("path", () => "./", "The path to search for a workspace")
        );

        var t_ConfigureCmd = t_WorkspaceCmd.NewSub("configure", "Configures a workspace");
        var t_DataCmd = t_ConfigureCmd.NewSub("data", "Configures workspace data");
        t_DataCmd.NewSub("add", "Adds data to workspace. This will replace an already existing key.", (Key, Json) => {
                t_WorkspaceService.FindWorkspace().Then(WS => {
                    var t_Data = WS.Workspace.Data;
                    JsonNode.Parse(Json).ToMaybe()
                        .Then(J => t_Data[Key] = J!);
                    t_WorkspaceService.SaveWorkspace(WS.Workspace with { Data = t_Data });
                });
            },
            new Argument<string>("key", "The key of the data to add"),
            new Argument<string>("json", "The json data to add")
        );

        t_DataCmd.NewSub("remove", "Removes data from workspace", (Key) => 
                t_WorkspaceService.FindWorkspace().Then(WS => {
                    var t_Data = WS.Workspace.Data;
                    t_Data.Remove(Key);
                    t_WorkspaceService.SaveWorkspace(WS.Workspace with { Data = t_Data });
                })
            ,
            new Argument<string>("key", "The key of the data to remove")
        );
        
    }
}