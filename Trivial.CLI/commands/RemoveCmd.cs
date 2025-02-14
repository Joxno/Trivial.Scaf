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
        var t_NameArg = new Argument<string>("name", "The name of the scaffold to uninstall");
        t_RemoveCmd.Add(t_NameArg);
        t_RemoveCmd.SetHandler(async (Name) => {
            var t_Service = new TemplateService();
            var t_Result = t_Service.UninstallTemplate(Name);
            t_Result.Then(_ => {
                Console.WriteLine("Template Removed.");
            },
            E => {
                Console.WriteLine(E.Message);
            });
        }, t_NameArg);

        Cmd.Add(t_RemoveCmd);
    }
}