using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class RemoveCmd
{
    public static void AddRemoveCmd(this RootCommand Cmd)
    {
        var t_RemoveCmd = new Command("remove", "Uninstalls a scaffold");
        var t_KeyArg = new Argument<string>("key", "The key of the scaffold to uninstall");
        t_RemoveCmd.Add(t_KeyArg);
        t_RemoveCmd.SetHandler(async (Key) => {
            var t_Service = Locator.GetTemplateService();
            var t_Result = t_Service.UninstallTemplate(Key);
            t_Result.Then(_ => {
                Console.WriteLine("Template Removed.");
            },
            E => {
                Console.WriteLine(E.Message);
            });
        }, t_KeyArg);

        Cmd.Add(t_RemoveCmd);
    }
}