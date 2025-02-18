using System.Text.Json;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.repositories;

public class TemplateRepository : ITemplateRepository
{
    private List<string> m_InstalledTemplatesPaths = new();
    private List<Template> m_Templates = new();
    private Dictionary<string, string> m_TemplatePaths = new();

    public TemplateRepository(List<string> InstalledTemplatesPaths)
    {
        m_InstalledTemplatesPaths = InstalledTemplatesPaths;
        _IndexTemplates();
    }

    private Result<Unit> _IndexTemplates() => Try.Invoke(() =>
    {
        var t_Templates = m_InstalledTemplatesPaths.SelectMany(_IndexTemplatesAtPath).ToList();
        foreach(var t_Indexed in t_Templates)
        {
            m_Templates.Add(t_Indexed.T);
            m_TemplatePaths.Add(t_Indexed.T.Key, t_Indexed.P);
        }
    });

    public Result<Unit> DeleteTemplate(Template Template) =>
        DeleteTemplate(Template.Key);

    public Result<Unit> DeleteTemplate(string Key) => Try.Invoke(() => {
        var t_TemplatePath = m_TemplatePaths[Key];
        if(Directory.Exists(t_TemplatePath)) Directory.Delete(t_TemplatePath, true);
        m_Templates.RemoveAll(T => T.Key == Key);
        m_TemplatePaths.Remove(Key);
    });

    public Maybe<Template> GetTemplate(string Key) =>
        m_Templates.FirstOrNone(T => T.Key == Key);

    public Maybe<string> GetTemplatePath(Template Template) =>
        GetTemplatePath(Template.Key);

    public List<Template> GetTemplates() => 
        m_Templates;

    private List<(Template T, string P)> _IndexTemplatesAtPath(string Path)
    {
        var t_TemplateModels = new List<(Template, string)>();

        var t_Templates = Directory.GetDirectories(Path);
        foreach (var t_Template in t_Templates)
        {
            var t_TemplateFile = System.IO.Path.Combine(t_Template, "template.scaf.json");
            if(File.Exists(t_TemplateFile))
            {
                Try.Invoke(() => {
                    var t_TemplateJson = File.ReadAllText(t_TemplateFile);
                    var t_TemplateModel = JsonSerializer.Deserialize<Template>(t_TemplateJson);
                    t_TemplateModels.Add((t_TemplateModel, t_Template));
                });
            }
        }

        return t_TemplateModels;
    }

    public Maybe<string> GetTemplatePath(string Key) =>
        m_TemplatePaths.Retrieve(Key);

    public Result<Template> InstallTemplate(string Path, bool Force) => Try.Invoke(() => {
        var t_TemplateFile = System.IO.Path.Combine(Path, "template.scaf.json");
        if(!File.Exists(t_TemplateFile)) throw new Exception("'template.scaf.json' not found.");

        var t_TemplateJson = File.ReadAllText(t_TemplateFile);
        var t_TemplateModel = JsonSerializer.Deserialize<Template>(t_TemplateJson);

        var t_TemplatesDir = ScafPaths.GetTemplatesPath();
        var t_TemplateDir = System.IO.Path.Combine(t_TemplatesDir, t_TemplateModel.Key);
        if(Directory.Exists(t_TemplateDir) && !Force) throw new Exception("Template already exists. Use --force to overwrite.");
        if(Directory.Exists(t_TemplateDir) && Force) Directory.Delete(t_TemplateDir, true);
        
        ScafPaths.CopyTemplate(Path, t_TemplateDir);
        m_Templates.Add(t_TemplateModel);
        m_TemplatePaths[t_TemplateModel.Key] = t_TemplateDir;

        return t_TemplateModel;
    });

    public Result<Template> InstallScript(string Path) => Try.Invoke(() => {
            if(!File.Exists(Path)) throw new Exception("File not found.");
            if(!Path.EndsWith(".ps1")) throw new Exception("File must be a PowerShell script.");
            var t_ScriptName = System.IO.Path.GetFileNameWithoutExtension(Path);
            var t_TemplateModel = new Template(t_ScriptName.ToLower(), t_ScriptName.ToLower(), "PowerShell Script", new(), new());

            var t_TemplatesDir = ScafPaths.GetTemplatesPath();
            var t_TemplateDir = System.IO.Path.Combine(t_TemplatesDir, t_TemplateModel.Name);
            if(Directory.Exists(t_TemplateDir)) throw new Exception("Template already exists.");

            Directory.CreateDirectory(t_TemplateDir);
            File.Copy(Path, System.IO.Path.Combine(t_TemplateDir, $"{t_ScriptName}.ps1"));
            var t_TemplateJson = JsonSerializer.Serialize(t_TemplateModel, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(System.IO.Path.Combine(t_TemplateDir, "template.scaf.json"), t_TemplateJson);

            m_Templates.Add(t_TemplateModel);
            m_TemplatePaths[t_TemplateModel.Key] = t_TemplateDir;

            return t_TemplateModel;
        });
}