using System.Diagnostics;

namespace Trivial.CLI.extensions;

public static class StringExtensions
{
    public static void RunAsTerminalCmd(this string Cmd)
    {
        using var t_Process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "pwsh",
                Arguments = $"-Command \"& {Cmd.EscapeQuotes()}\"",
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
}