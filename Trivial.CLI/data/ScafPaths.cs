namespace Trivial.CLI.data;

public static class ScafPaths
{
    public static string GetRootPath() =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Trivial.Scaf");

    public static string GetTemplatesPath() =>
        Path.Combine(GetRootPath(), "templates");

    public static string GetConfigPath() =>
        Path.Combine(GetRootPath(), "cfg");

    public static string[] GetInitPaths() =>
        [ 
            GetRootPath(), 
            GetTemplatesPath(), 
            GetConfigPath()
        ];

    public static string ResolvePath(string Path) =>
        System.IO.Path.IsPathFullyQualified(Path) ? Path : System.IO.Path.Combine(Environment.CurrentDirectory, Path);

    public static string ResolvePath(string Path, string WorkingDir) =>
        System.IO.Path.IsPathFullyQualified(Path) ? Path : System.IO.Path.Combine(WorkingDir, Path);

    public static void CopyTemplate(string Path, string Destination)
    {
        _CopyDirectoryRecursively(Path, Destination);
    }

    private static void _CopyDirectoryRecursively(string Source, string Destination)
    {
        var t_Dir = new DirectoryInfo(Source);
        
        if (!t_Dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {Source}");

        DirectoryInfo[] t_Dirs = t_Dir.GetDirectories();
        Directory.CreateDirectory(Destination);

        foreach (FileInfo t_File in t_Dir.GetFiles())
        {
            string t_TargetFilePath = Path.Combine(Destination, t_File.Name);
            t_File.CopyTo(t_TargetFilePath, true);
        }

        foreach (DirectoryInfo t_SubDir in t_Dirs)
        {
            string t_NewDestinationDir = Path.Combine(Destination, t_SubDir.Name);
            _CopyDirectoryRecursively(t_SubDir.FullName, t_NewDestinationDir);
        }
    }
}