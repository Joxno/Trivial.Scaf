using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class InstallCmd
{
    public static void AddInstallCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetTemplateService();

        var t_InstallCmd = Cmd.NewSub("install", "Installs a scaffold");
        var t_TemplateCmd = t_InstallCmd.NewSub("template", "Installs a template", (Path, Force, FromPath) => 
            {
                if(FromPath)
                    t_Service.InstallTemplate(ScafPaths.ResolvePath(Path), Force)
                    .Then(
                        _ => Console.WriteLine("Template Installed."),
                        E => Console.WriteLine(E.Message)
                    );
                else
                    t_Service.InstallTemplateFromRepo(Path, Force)
                    .Then(
                        _ => Console.WriteLine("Template Installed."),
                        E => Console.WriteLine(E.Message)
                    );
            },
            new Argument<string>("path", "The path to the template"),
            new Option<bool>(["--force", "-f"], () => false, "Forces the installation"),
            new Option<bool>(["--from-path", "-p"], () => false, "Installs from a path")
        );

        var t_ScriptCmd = t_InstallCmd.NewSub("script", "Installs a script", Path => 
                t_Service.InstallScript(ScafPaths.ResolvePath(Path))
                .Then(
                    _ => Console.WriteLine("Script Installed."),
                    E => Console.WriteLine(E.Message)
                ),
            new Argument<string>("path", "The path to the script")
        );
    }
}