using System.CommandLine;
using Trivial.CLI.commands;

namespace Trivial.CLI;

public static class Commands
{
    public static void AddCommands(this RootCommand Root)
    {
        Root.AddListCmd();
        Root.AddFolderCmd();
    }
}