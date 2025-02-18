using System.CommandLine;
using System.Text.Json;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.models;

namespace Trivial.CLI.commands;

public static class InitCmd
{
    public static void AddInitCmd(this RootCommand Cmd)
    {
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
        });

        var t_InitTemplateCmd = t_InitCmd.NewSub("template", "Initialises a template", 
            async (Name, Key, Description, Output) => {
                var t_NewTemplate = new Template(Name, Key, Description,
                    new(new(), new()),
                    [ new("", "", [], [], new("Run-Default.ps1", ["Includes.ps1"], [new("ScafCfg", "template.scaf.json")])) ]);
                var t_Serialized = JsonSerializer.Serialize(t_NewTemplate, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(Path.Combine(Output, $"template.scaf.json"), t_Serialized);
                await File.WriteAllTextAsync(Path.Combine(Output, "Run-Default.ps1"), "Hello-World");
                await File.WriteAllTextAsync(Path.Combine(Output, "Includes.ps1"), "function Hello-World() {\n\tWrite-Host \"Hello, World! from $($ScafCfg.Name) template.\"\n}");
            },
            new Argument<string>("name", "The name of the template"),
            new Argument<string>("key", "The key for the template"),
            new Option<string>(["-d", "--description"], "The description of the template"),
            new Option<string>(["-o", "--output"], "The output directory for the template")
        );
    }
}