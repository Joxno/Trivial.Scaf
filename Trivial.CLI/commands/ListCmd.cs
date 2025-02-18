using System.CommandLine;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class ListCmd
{
    public static void AddListCmd(this RootCommand Cmd)
    {
        var t_ListCmd = Cmd.NewSub("list", "Lists objects");

        var t_ListTemplates = t_ListCmd.NewSub("templates", "Lists available templates", () => {
            var t_Service = Locator.GetTemplateService();
            var t_Templates = t_Service.GetTemplates();
            Console.WriteLine("Name\t\tKey\t\tDescription");
            foreach(var t_Template in t_Templates)
            {
                Console.WriteLine($"{t_Template.Name}\t\t{t_Template.Key}\t\t{t_Template.Description}");
            }
        });

        var t_ListPwd = t_ListCmd.NewSub("pwd", "Lists the current working directory", () => {
            Console.WriteLine(Environment.CurrentDirectory);
        });

        var t_ListRepos = t_ListCmd.NewSub("repos", "Lists template repositories", () => {
            Console.WriteLine("No Repositories.");
        });

        var t_ListCfg = t_ListCmd.NewSub("cfg", "Lists config", () => {
            Console.WriteLine("No Config.");
        });

        var t_ListDirs = t_ListCmd.NewSub("dirs", "Lists directories", () => {
            ScafPaths.GetInitPaths().ToList().ForEach(t_Dir => Console.WriteLine(t_Dir));
        });
    }
}