using System.CommandLine;
using System.Text.Json;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.models;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class InitCmd
{
    public static void AddInitCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetSettingsService();
        var t_RepoService = Locator.GetRepoService();
        var t_InitCmd = Cmd.NewSub("init", "Initialises the scaf tool", () => {
            var t_Paths = ScafPaths.GetInitPaths();
            foreach(var t_Path in t_Paths)
            {
                if(!Directory.Exists(t_Path))
                {
                    Directory.CreateDirectory(t_Path);
                    Console.WriteLine($"Created Directory: {t_Path}");
                }
            }

            t_Service.Init();
        });

        var t_InitTemplateCmd = t_InitCmd.NewSub("template", "Initialises a template", 
            async (Name, Key, Description, Output) => {
                var t_ResolvedPath = ScafPaths.ResolvePath(Output);
                if(!Directory.Exists(t_ResolvedPath))
                    Directory.CreateDirectory(t_ResolvedPath);

                var t_NewTemplate = new Template(Guid.NewGuid(), Name, Key, Description,
                    new(new(), new()),
                    [ new("", "", [], [], new("Run-Default.ps1", ["Includes.ps1"], [new("ScafCfg", "template.scaf.json")])) ]);
                var t_Serialized = JsonSerializer.Serialize(t_NewTemplate, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(Path.Combine(t_ResolvedPath, $"template.scaf.json"), t_Serialized);
                await File.WriteAllTextAsync(Path.Combine(t_ResolvedPath, "Run-Default.ps1"), "Hello-World");
                await File.WriteAllTextAsync(Path.Combine(t_ResolvedPath, "Includes.ps1"), "function Hello-World() {\n\tWrite-Host \"Hello, World! from $($ScafCfg.Name) template.\"\n}");
            },
            new Argument<string>("name", "The name of the template"),
            new Argument<string>("key", "The key for the template"),
            new Option<string>(["-d", "--description"], "The description of the template"),
            new Option<string>(["-o", "--output"], () => "./", "The output directory for the template")
        );

        var t_InitRepoCmd = t_InitCmd.NewSub("repo", "Initialises a new remote repo", (Path, AddRemote, Name) => {
            var t_ResolvedPath = ScafPaths.ResolvePath(Path);
            var t_Result = t_RepoService.InitRepo(t_ResolvedPath, Name!);
            t_Result.Then(
                R => {
                    Console.WriteLine("Repo Initialised.");
                    if(AddRemote)
                        t_RepoService.AddRemoteRepo(t_ResolvedPath, R.Name);
                },
                E => Console.WriteLine(E.Message)
            );
        }, 
        new Argument<string>("path", "The path to the repo. Creates the path if it doesn't already exist."),
        new Option<bool>(["--add-remote"], "Adds the initialised repo to scaf settings for remote repos."),
        new Option<string?>(["--name", "-n"], "The name of the repo")
        );
    }
}