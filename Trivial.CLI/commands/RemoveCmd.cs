using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class RemoveCmd
{
    public static void AddRemoveCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetTemplateService();
        var t_RemoveCmd = Cmd.NewSub("remove", "Uninstalls a scaffold", Key => {
                var t_Result = t_Service.UninstallTemplate(Key);
                t_Result.Then(
                    _ => Console.WriteLine("Template Removed."),
                    E => Console.WriteLine(E.Message)
                );
            },
            new Argument<string>("key", "The key of the scaffold to uninstall")
        );
    }
}