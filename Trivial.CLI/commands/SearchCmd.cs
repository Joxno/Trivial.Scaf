using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.models;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class SearchCmd
{
    public static void AddSearchCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetSearchService();
        var t_WorkSpaceService = Locator.GetWorkspaceService();
        var t_SearchCmd = Cmd.NewSub("search", "Search commands");
        t_SearchCmd.NewSub("template", "Searches for a template", (Query, ById, ByName) => {
                var t_Templates = ById ? 
                    t_Service.SearchForTemplateById(Guid.Parse(Query)) : ByName ? 
                    t_Service.SearchForTemplateByName(Query) : 
                    t_Service.SearchForTemplateByKey(Query);

                var t_Table = new ConsoleTable("Id", "Name", "Key");
                foreach(var t_Template in t_Templates)
                {
                    t_Table.AddRow(t_Template.Id, t_Template.Name, t_Template.Key);
                }

                t_Table.Print();
            }, 
            new Argument<string>("query", "The query to search for"),
            new Option<bool>(["--by-id", "-i"], () => false, "Searches by id"),
            new Option<bool>(["--by-name", "-n"], () => false, "Searches by name")
        );

        t_SearchCmd.NewSub("workspace", "Searches for the closest workspace", () => {
            
        });
    }
}