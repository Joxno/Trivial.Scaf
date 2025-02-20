using System.CommandLine;
using Trivial.CLI.commands;
using Trivial.CLI.extensions;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;
using Trivial.CLI.services;
using Trivial.CLI.data;
using System.CommandLine.Parsing;

namespace Trivial.CLI;

public static class TemplateCommands
{
    public static void AddTemplateCommands(this RootCommand Root) => Try.Invoke(() =>
    {
        var t_Service = Locator.GetTemplateService();
        var t_Templates = t_Service.GetTemplates();
        foreach(var t_Template in t_Templates)
        {
            var t_TemplateCmd = new Command(t_Template.Key, $"Runs the {t_Template.Name} scaffold");

            foreach(var t_Trigger in t_Template.Triggers)
            {
                _TryAddRun(t_Trigger, t_TemplateCmd, t_Template, t_Service);
            }

            Root.Add(t_TemplateCmd);
        }
    });

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

        Cmd.SetHandler(C =>
        {
            var t_TemplateDir = Service.GetTemplatePath(Template.Key).ValueOr("");
            var t_Args = _ConstructArgsList(Cmd, C);
            var t_Params = _ConstructParamsList(Cmd, C);

            var t_GlobalCfgIncludes = string.Join("", Template.Global.Configs
                .Select(Cfg => $"${Cfg.Name} = (Get-Content {ScafPaths.ResolvePath(Cfg.File, t_TemplateDir)} | ConvertFrom-Json -Depth 99);").ToList());

            var t_GlobalIncludes = string.Join("", Template.Global.Includes
                .Select(Path => $". \"{ScafPaths.ResolvePath(Path, t_TemplateDir)}\";").ToList());

            var t_CfgIncludes = string.Join("", Trigger.Action.Configs
                .Select(Cfg => $"${Cfg.Name} = (Get-Content {ScafPaths.ResolvePath(Cfg.File, t_TemplateDir)} | ConvertFrom-Json -Depth 99);").ToList());
            var t_Includes = string.Join("", Trigger.Action.Includes
                .Select(Path => $". \"{ScafPaths.ResolvePath(Path, t_TemplateDir)}\";").ToList());

            var t_Executable = System.IO.Path.Combine(t_TemplateDir, Trigger.Action.Executable);
            var t_ArgsStr = string.Join(" ", t_Args.Select(A => $"-{A.Item1} {A.Item2}"));
            var t_ParamStr = string.Join(" ", t_Params.Select(P => $"-{P.Item1} {P.Item2}"));
            var t_ExecutionStr = string.Join("", [
                "{",
                t_GlobalCfgIncludes,
                t_GlobalIncludes,
                t_CfgIncludes,
                t_Includes,
                $". \"{t_Executable}\" {t_ArgsStr} {t_ParamStr};",
                "}"
            ]);
            //Console.WriteLine(t_ExecutionStr);
            t_ExecutionStr.RunAsTerminalCmdWithPwd(Maybe.None);

        });
    });

    private static List<(string, string)> _ConstructParamsList(Command Cmd, System.CommandLine.Invocation.InvocationContext C) =>
        Cmd.Options
            .Where(O => C.ParseResult.FindResultFor(O) is OptionResult)
            .Select(O => (O.Name, C.ParseResult.GetValueForOption(O)?.ToString() ?? "")).ToList();

    private static List<(string, string)> _ConstructArgsList(Command Cmd, System.CommandLine.Invocation.InvocationContext C) =>
        Cmd.Arguments
            .Where(O => C.ParseResult.FindResultFor(O) is ArgumentResult)
            .Select(A => (A.Name, C.ParseResult.GetValueForArgument(A)?.ToString() ?? "")).ToList();
}