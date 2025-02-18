using System.Diagnostics;
using System.Text.Json;

namespace Trivial.CLI.extensions;

public static class StringExtensions
{
    public static void RunAsTerminalCmd(this string Cmd)
    {
        var t_CmdArg = $"-Command \"& {Cmd.EscapeQuotes()}\"";
        //Console.WriteLine(t_CmdArg);
        using var t_Process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pwsh",
                Arguments = t_CmdArg,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        t_Process.Start();
        Console.WriteLine(t_Process.StandardOutput.ReadToEnd());
        t_Process.WaitForExit();
    }

    public static string EscapeQuotes(this string Str) => 
        Str.Replace("\"", "\\\"");

    public static string ToJson<T>(this T Obj) => 
        JsonSerializer.Serialize(Obj, new JsonSerializerOptions { WriteIndented = true });
}