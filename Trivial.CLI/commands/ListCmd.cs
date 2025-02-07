using System.CommandLine;

namespace Trivial.CLI.commands;

public static class ListCmd
{
    public static void AddListCmd(this RootCommand Cmd)
    {
        var t_ListCmd = new Command("list", "Lists objects");
        var t_ListTemplates = new Command("templates", "Lists available templates");
        t_ListTemplates.SetHandler(async () => {
            Console.WriteLine("No Installed Templates.");
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

        t_ListCmd.Add(t_ListTemplates);
        t_ListCmd.Add(t_ListPwd);
        t_ListCmd.Add(t_ListRepos);
        t_ListCmd.Add(t_ListCfg);
        Cmd.Add(t_ListCmd);
    }
}