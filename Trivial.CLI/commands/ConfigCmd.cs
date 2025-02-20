using System.CommandLine;
using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.interfaces;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class ConfigCmd
{
    public static void AddConfigCmd(this RootCommand RootCmd)
    {
        var t_Service = Locator.GetSettingsService();
        var t_ConfigCmd = RootCmd.NewSub("config", "Used to modify the scaf config.");
        t_ConfigCmd.NewSub("add", "Adds config")._AddCmd(t_Service);
        t_ConfigCmd.NewSub("remove", "Removed config")._RemoveCmd(t_Service);
        t_ConfigCmd.NewSub("set", "Sets config")._SetCmd(t_Service);
    }

    private static void _AddCmd(this Command Cmd, ISettingsService Service)
    {
        Cmd.NewSub("templates-path", "Adds a templates path", Path => 
            Service.GetTemplatesConfig()
                .Map(C => C with { InstalledTemplatesPaths = C.InstalledTemplatesPaths.Append(ScafPaths.ResolvePath(Path)).ToList() })
                .Bind(Service.SaveTemplatesConfig)
                .Then(
                    _ => Console.WriteLine("Templates Path Added."),
                    E => Console.WriteLine(E.Message)
                ), 
            new Argument<string>("path", "The path to the templates")
        );

        Cmd.NewSub("workspace", "Adds workspace config", (Id, Name, Path) => {
                var t_WorkspaceConfig = new WorkspaceRef(Guid.Parse(Id), Name, ScafPaths.ResolvePath(Path));
                Service.SaveWorkspacesConfig(t_WorkspaceConfig)
                    .Then(
                        _ => Console.WriteLine("Workspace Added."),
                        E => Console.WriteLine(E.Message)
                    );
            },
            new Argument<string>("id", "The id of the workspace"),
            new Argument<string>("name", "The name of the workspace"),
            new Argument<string>("path", "The path to the workspace")
        );
    }

    private static void _RemoveCmd(this Command Cmd, ISettingsService Service)
    {

    }

    private static void _SetCmd(this Command Cmd, ISettingsService Service)
    {

    }
}