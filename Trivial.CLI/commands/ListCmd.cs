using System.CommandLine;
using Trivial.CLI.data;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class ListCmd
{
    public static void AddListCmd(this RootCommand Cmd)
    {
        var t_ListCmd = new Command("list", "Lists objects");
        var t_ListTemplates = new Command("templates", "Lists available templates");
        t_ListTemplates.SetHandler(async () => {
            var t_Service = new TemplateService();
            var t_Templates = t_Service.GetTemplates();
            Console.WriteLine("Name\t\tKey\t\tDescription");
            foreach(var t_Template in t_Templates)
            {
                Console.WriteLine($"{t_Template.Name}\t\t{t_Template.Key}\t\t{t_Template.Description}");
            }
        });

        var t_ListPwd = new Command("pwd", "Lists the current working directory");
        t_ListPwd.SetHandler(async () => {
            Console.WriteLine(Environment.CurrentDirectory);
        });

        var t_ListRepos = new Command("repos", "Lists template repositories");
        t_ListRepos.SetHandler(async () => {
            Console.WriteLine("No Repositories.");
        });

        var t_ListCfg = new Command("cfg", "Lists config");
        t_ListCfg.SetHandler(async () => {
            Console.WriteLine("No Config.");
        });

        var t_ListDirs = new Command("dirs", "Lists directories");
        t_ListDirs.SetHandler(async () => {
            var t_Dirs = ScafPaths.GetInitPaths();
            foreach(var t_Dir in t_Dirs)
            {
                Console.WriteLine(t_Dir);
            }
        });

        t_ListCmd.Add(t_ListDirs);
        t_ListCmd.Add(t_ListTemplates);
        t_ListCmd.Add(t_ListPwd);
        t_ListCmd.Add(t_ListRepos);
        t_ListCmd.Add(t_ListCfg);
        Cmd.Add(t_ListCmd);
    }
}