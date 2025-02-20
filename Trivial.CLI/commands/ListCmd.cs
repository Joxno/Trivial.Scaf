using System.Collections;
using System.CommandLine;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.models;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class ListCmd
{
    public static void AddListCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetSettingsService();
        var t_RepoService = Locator.GetRepoService();
        var t_WorkspaceService = Locator.GetWorkspaceService();
        var t_ListCmd = Cmd.NewSub("list", "Lists objects");

        var t_ListTemplates = t_ListCmd.NewSub("templates", "Lists locally available templates", (RepoName) => {
            var t_Service = Locator.GetTemplateService();
            var t_Name = RepoName.ToMaybe();

            var t_Table = new ConsoleTable("Name", "Key", "Description");

            if(!t_Name.HasValue)
            {
                var t_Templates = t_Service.GetTemplates();
                foreach(var t_Template in t_Templates)
                {
                    t_Table.AddRow(t_Template.Name, t_Template.Key, t_Template.Description);
                }
            }
            else
            {
                t_RepoService.GetIndexByName(t_Name.Value!).Map(I => I.Templates)
                    .Then(Templates => {
                        foreach(var t_Template in Templates)
                        {
                            t_Table.AddRow(t_Template.Name, t_Template.Key, t_Template.Path);
                        }
                    });
            }

            t_Table.Print();
        },
        new Option<string?>(["--from-repo", "-r"], "The repository to list templates from")
        );

        var t_ListVars = t_ListCmd.NewSub("vars", "Lists context variables");
        var t_ListEnvVars = t_ListVars.NewSub("env", "Lists environment variables", () => {
            foreach(DictionaryEntry t_Var in Environment.GetEnvironmentVariables())
            {
                Console.WriteLine($"{t_Var.Key}={t_Var.Value}");
            }
        });

        var t_ListScafVars = t_ListVars.NewSub("scaf", "Lists scaf variables", () => {
            
        });

        var t_ListPwd = t_ListCmd.NewSub("pwd", "Lists the current working directory", () => {
            Console.WriteLine(Environment.CurrentDirectory);
        });

        var t_ListRepos = t_ListCmd.NewSub("repos", "Lists template repositories", () => {
            var t_Repos = t_RepoService.GetLocalIndexes();
            var t_Table = new ConsoleTable("Name", "Url", "Count", "Id");
            foreach(var t_Repo in t_Repos)
            {
                t_Table.AddRow(t_Repo.Name, t_Repo.Url, t_Repo.Templates.Count, t_Repo.Id);
            }

            t_Table.Print();
        });

        var t_ListWorkspaces = t_ListCmd.NewSub("workspaces", "Lists workspaces", () => {
            var t_Workspaces = t_WorkspaceService.GetWorkspaces();
            var t_Table = new ConsoleTable("Id", "Name", "Path");
            foreach(var t_Workspace in t_Workspaces)
            {
                t_Table.AddRow(t_Workspace.Id, t_Workspace.Name, t_WorkspaceService.GetWorkspacePath(t_Workspace.Id).ValueOr("???"));
            }

            t_Table.Print();
        });

        var t_ListCfg = t_ListCmd.NewSub("cfg", "Lists config");
        var t_ListAllCfg = t_ListCfg.NewSub("all", "Lists full config file", () => {
            var t_Config = t_Service.GetToolConfig();
            t_Config.Then(
                Cfg => Console.WriteLine(Cfg.ToJson()),
                E => Console.WriteLine(E.Message)
            );
        });
        var t_ListReposCfg = t_ListCfg.NewSub("repos", "Lists repository config", () => {
            var t_Config = t_Service.GetReposConfig();
            t_Config.Then(
                Cfg => Console.WriteLine(Cfg.ToJson()),
                E => Console.WriteLine(E.Message)
            );
        });
        var t_ListTemplatesCfg = t_ListCfg.NewSub("templates", "Lists templates config", () => {
            var t_Config = t_Service.GetTemplatesConfig();
            t_Config.Then(
                Cfg => Console.WriteLine(Cfg.ToJson()),
                E => Console.WriteLine(E.Message)
            );
        });

        var t_ListWorkspacedCfg = t_ListCfg.NewSub("workspaces", "Lists workspaces config", () => {
            var t_Config = t_Service.GetToolConfig();
            t_Config.Then(
                Cfg => Console.WriteLine(Cfg.Workspaces.ToJson()),
                E => Console.WriteLine(E.Message)
            );
        });

        var t_ListDirs = t_ListCmd.NewSub("dirs", "Lists directories", () => {
            ScafPaths.GetInitPaths().ToList().ForEach(t_Dir => Console.WriteLine(t_Dir));
        });
    }
}