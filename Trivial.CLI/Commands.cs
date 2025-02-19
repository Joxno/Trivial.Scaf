using System.CommandLine;
using Trivial.CLI.commands;

namespace Trivial.CLI;

public static class Commands
{
    public static void AddCommands(this RootCommand Root)
    {
        Root.AddListCmd();
        Root.AddFolderCmd();
        Root.AddInstallCmd();
        Root.AddInitCmd();
        Root.AddRemoveCmd();
        Root.AddIndexCmd();
        Root.AddConfigCmd();
        Root.AddRepoCmd();
        Root.AddSearchCmd();
    }
}