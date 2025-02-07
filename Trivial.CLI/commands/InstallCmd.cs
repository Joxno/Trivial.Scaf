using System.CommandLine;
using System.IO;

namespace Trivial.CLI.commands;

public static class InstallCmd
{
    public static void AddInstallCmd(this RootCommand Cmd)
    {
        var t_InstallCmd = new Command("install", "Installs a scaffold");
        var t_TemplateCmd = new Command("template", "Installs a template");
        var t_ScriptCmd = new Command("script", "Installs a script");

        
        t_InstallCmd.Add(t_TemplateCmd);
        t_InstallCmd.Add(t_ScriptCmd);
        Cmd.Add(t_InstallCmd);
    }
}