using System.CommandLine;
using System.IO;

namespace Trivial.CLI.commands;

public static class FolderCmd
{
    public static void AddFolderCmd(this RootCommand Cmd)
    {
        var t_FolderCmd = new Command("folder", "Scaffolds a single or multiple folders");
        var t_PathOption = new Argument<string[]>("paths", "The path to scaffold");
        t_FolderCmd.Add(t_PathOption);
        t_FolderCmd.SetHandler(async (Paths) => {
            foreach(var P in Paths)
            {
                var t_ResolvedPath = Path.IsPathFullyQualified(P) ? P : System.IO.Path.Combine(Environment.CurrentDirectory, P);
                Try.Invoke(() => Directory.CreateDirectory(t_ResolvedPath))
                    .Then(DI => {
                        Console.WriteLine($"Created Directory: {DI.FullName}");
                    },
                    E => Console.WriteLine(E.Message));
            }
        }, t_PathOption);

        
        Cmd.Add(t_FolderCmd);
    }
}