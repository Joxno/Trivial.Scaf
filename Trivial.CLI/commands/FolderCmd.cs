using System.CommandLine;
using Trivial.CLI.data;
using Trivial.CLI.extensions;

namespace Trivial.CLI.commands;

public static class FolderCmd
{
    public static void AddFolderCmd(this RootCommand RootCmd) => 
        RootCmd.NewSub("folder", "Scaffolds single or multiple folders", Paths =>
            Paths.ForEach(P => {
                var t_ResolvedPath = ScafPaths.ResolvePath(P);
                Try.Invoke(() => Directory.CreateDirectory(t_ResolvedPath))
                    .Then(
                        DI => Console.WriteLine($"Created Directory: {DI.FullName}"),
                        E => Console.WriteLine(E.Message)
                    );
            }),
            new Argument<string[]>("paths", "The path to scaffold")
        );
}