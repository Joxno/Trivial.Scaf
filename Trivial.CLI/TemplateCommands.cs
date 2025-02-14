using System.CommandLine;
using Trivial.CLI.commands;
using Trivial.CLI.extensions;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;
using Trivial.CLI.services;

namespace Trivial.CLI;

public static class TemplateCommands
{
    public static void AddTemplateCommands(this RootCommand Root)
    {
        var t_Service = new TemplateService();
        var t_Templates = t_Service.GetTemplates();
        foreach(var t_Template in t_Templates)
        {
            var t_TemplateCmd = new Command(t_Template.Key, $"Runs the {t_Template.Name} template");

            foreach(var t_Trigger in t_Template.Triggers)
            {
                _TryAddRun(t_Trigger, t_TemplateCmd, t_Template, t_Service);
            }

            Root.Add(t_TemplateCmd);
        }
    }

    private static void _TryAddRun(TemplateRun Trigger, Command TemplateCmd, Template Template, ITemplateService Service) => Try.Invoke(() => {
        if(string.IsNullOrWhiteSpace(Trigger.Keyword))
        {
            _ConstructCmdProxy(Trigger, TemplateCmd, Template, Service);
        }
        else
        {
            var t_RunCmd = new Command(Trigger.Keyword, Trigger.Description);
            _ConstructCmdProxy(Trigger, t_RunCmd, Template, Service);

            TemplateCmd.Add(t_RunCmd);
        }
    });

    private static void _ConstructCmdProxy(TemplateRun Trigger, Command Cmd, Template Template, ITemplateService Service) => Try.Invoke(() => {
        Trigger.Parameters.ForEach(P => {
            var t_Param = new Argument<string>(P.Name, P.Description);
            Cmd.Add(t_Param);
        });

        Trigger.Options.ForEach(O => {
            var t_Option = new Option<string>(O.Names.ToArray(), O.Description);
            Cmd.Add(t_Option);
        });

        Cmd.SetHandler(C => {
            var t_TemplateDir = Service.GetTemplatePath(Template.Name).ValueOr("");
            var t_Args = new List<(string, string)>();
            foreach(var t_Arg in Cmd.Arguments)
            {
                t_Args.Add((t_Arg.Name, C.ParseResult.GetValueForArgument(t_Arg)?.ToString() ?? ""));
            }

            var t_Params = new List<(string, string)>();
            foreach(var t_Opt in Cmd.Options)
            {
                t_Params.Add((t_Opt.Name, C.ParseResult.GetValueForOption(t_Opt)?.ToString() ?? ""));
            }

            var t_Executable = System.IO.Path.Combine(t_TemplateDir, Trigger.Action.Executable);
            var t_ArgsStr = string.Join(" ", t_Args.Select(A => $"-{A.Item1} {A.Item2}"));
            var t_ParamStr = string.Join(" ", t_Params.Select(P => $"-{P.Item1} {P.Item2}"));
            var t_ExecutionStr = $"{{$Inject = \"injected data\"; . \"{t_Executable}\" {t_ArgsStr} {t_ParamStr}}}";
            Console.WriteLine(t_ExecutionStr);
            t_ExecutionStr.RunAsTerminalCmd();
            
        });
    });
}