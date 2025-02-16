using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class InstallCmd
{
    public static void AddInstallCmd(this RootCommand Cmd)
    {
        var t_InstallCmd = new Command("install", "Installs a scaffold");
        var t_TemplateCmd = new Command("template", "Installs a template");
        var t_TemplatePath = new Argument<string>("path", "The path to the template");
        var t_TemplateForce = new Option<bool>(["--force", "-f"], "Forces the installation");
        t_TemplateForce.SetDefaultValue(false);
        t_TemplateCmd.Add(t_TemplatePath);
        t_TemplateCmd.Add(t_TemplateForce);
        t_TemplateCmd.SetHandler(async (Path, Force) => {
            var t_ResolvedPath = ScafPaths.ResolvePath(Path);
            var t_Service = Locator.GetTemplateService();
            var t_Result = t_Service.InstallTemplate(t_ResolvedPath, Force);
            t_Result.Then(_ => {
                Console.WriteLine("Template Installed.");
            },
            E => {
                Console.WriteLine(E.Message);
            });
        }, t_TemplatePath, t_TemplateForce);

        var t_ScriptCmd = new Command("script", "Installs a script");
        var t_ScriptPath = new Argument<string>("path", "The path to the script");
        t_ScriptCmd.Add(t_ScriptPath);
        t_ScriptCmd.SetHandler(async (Path) => {
            var t_ResolvedPath = ScafPaths.ResolvePath(Path);
            var t_Service = Locator.GetTemplateService();
            var t_Result = t_Service.InstallScript(t_ResolvedPath);
            t_Result.Then(_ => {
                Console.WriteLine("Script Installed.");
            },
            E => {
                Console.WriteLine(E.Message);
            });
        }, t_ScriptPath);
        
        t_InstallCmd.Add(t_TemplateCmd);
        t_InstallCmd.Add(t_ScriptCmd);
        Cmd.Add(t_InstallCmd);
    }
}