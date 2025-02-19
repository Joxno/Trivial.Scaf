using System.CommandLine;
using System.IO;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.services;

namespace Trivial.CLI.commands;

public static class SearchCmd
{
    public static void AddSearchCmd(this RootCommand Cmd)
    {
        var t_Service = Locator.GetSearchService();
        var t_SearchCmd = Cmd.NewSub("search", "Search commands");
        t_SearchCmd.NewSub("template", "Searches for a template", (Query, ById, ByName) => {
                var t_Templates = ById ? 
                    t_Service.SearchForTemplateById(Guid.Parse(Query)) : ByName ? 
                    t_Service.SearchForTemplateByName(Query) : 
                    t_Service.SearchForTemplateByKey(Query);

                Console.WriteLine("Id\t\tName\t\tKey");
                foreach(var t_Template in t_Templates)
                {
                    Console.WriteLine($"{t_Template.Id}\t\t{t_Template.Name}\t\t{t_Template.Key}");
                }
            }, 
            new Argument<string>("query", "The query to search for"),
            new Option<bool>(["--by-id", "-i"], () => false, "Searches by id"),
            new Option<bool>(["--by-name", "-n"], () => false, "Searches by name")
        );
    }
}