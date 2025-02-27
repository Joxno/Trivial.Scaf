﻿using System.CommandLine;

namespace Trivial.CLI;

public static class Program
{
    public static async Task<int> Main(string[] Args)
    {
        var t_RootCommand = new RootCommand();
        t_RootCommand.AddCommands();
        t_RootCommand.AddTemplateCommands();

        return await t_RootCommand.InvokeAsync(Args);
    }
}