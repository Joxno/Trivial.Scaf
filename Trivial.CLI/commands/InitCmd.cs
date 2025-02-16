using System.CommandLine;
using System.IO;
using System.Text.Json;
using Trivial.CLI.data;
using Trivial.CLI.models;

namespace Trivial.CLI.commands;

public static class InitCmd
{
    public static void AddInitCmd(this RootCommand Cmd)
    {
        var t_InitCmd = new Command("init", "Initialises the scaf tool");
        t_InitCmd.SetHandler(async () => {
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

        var t_InitTemplateCmd = new Command("template", "Initialises a template");
        var t_NameArg = new Argument<string>("name", "The name of the template");
        var t_KeyArg = new Argument<string>("key", "The key for the template");
        var t_DescOpt = new Option<string>(["-d", "--description"], "The description of the template");
        t_DescOpt.SetDefaultValue("");

        t_InitTemplateCmd.Add(t_NameArg);
        t_InitTemplateCmd.Add(t_KeyArg);
        t_InitTemplateCmd.Add(t_DescOpt);
        t_InitTemplateCmd.SetHandler(async (Name, Key, Description) => {
            var t_NewTemplate = new Template(Name, Key, Description, 
                [ new("", "", [], [], new("Run-Default.ps1", ["Includes.ps1"], [new("ScafCfg", "template.scaf.json")])) ]);
            var t_Serialized = JsonSerializer.Serialize(t_NewTemplate, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, $"template.scaf.json"), t_Serialized);
            await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Run-Default.ps1"), "Hello-World");
            await File.WriteAllTextAsync(Path.Combine(Environment.CurrentDirectory, "Includes.ps1"), "function Hello-World() {\n\tWrite-Host \"Hello, World! from $($ScafCfg.Name) template.\"\n}");
        }, t_NameArg, t_KeyArg, t_DescOpt);
        
        t_InitCmd.Add(t_InitTemplateCmd);
        Cmd.Add(t_InitCmd);
    }
}