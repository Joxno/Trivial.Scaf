using System.Text.Json;
using Trivial.CLI.data;
using Trivial.CLI.interfaces;
using Trivial.CLI.models;

namespace Trivial.CLI.services;

public class TemplateService : ITemplateService
{
    public List<Template> GetTemplates()
    {
        var t_TemplateModels = new List<Template>();

        var t_TemplatesDir = ScafPaths.GetTemplatesPath();
        var t_Templates = Directory.GetDirectories(t_TemplatesDir);
        foreach (var t_Template in t_Templates)
        {
            var t_TemplateFile = Path.Combine(t_Template, "template.scaf.json");
            if(File.Exists(t_TemplateFile))
            {
                Try.Invoke(() => {
                    var t_TemplateJson = File.ReadAllText(t_TemplateFile);
                    var t_TemplateModel = JsonSerializer.Deserialize<Template>(t_TemplateJson);
                    t_TemplateModels.Add(t_TemplateModel);
                });
            }
        }

        return t_TemplateModels;
    }

    public Result<Unit> InstallTemplate(string Path, bool Force) => Try.Invoke(() => {
        var t_TemplateFile = System.IO.Path.Combine(Path, "template.scaf.json");
        if(!File.Exists(t_TemplateFile)) throw new Exception("'template.scaf.json' not found.");

        var t_TemplateJson = File.ReadAllText(t_TemplateFile);
        var t_TemplateModel = JsonSerializer.Deserialize<Template>(t_TemplateJson);

        var t_TemplatesDir = ScafPaths.GetTemplatesPath();
        var t_TemplateDir = System.IO.Path.Combine(t_TemplatesDir, t_TemplateModel.Name);
        if(Directory.Exists(t_TemplateDir) && !Force) throw new Exception("Template already exists. Use --force to overwrite.");
        if(Directory.Exists(t_TemplateDir) && Force) Directory.Delete(t_TemplateDir, true);
        
        ScafPaths.CopyTemplate(Path, t_TemplateDir);
    });

    public Result<Unit> InstallScript(string Path) => Try.Invoke(() => {
        if(!File.Exists(Path)) throw new Exception("File not found.");
        if(!Path.EndsWith(".ps1")) throw new Exception("File must be a PowerShell script.");
        var t_ScriptName = System.IO.Path.GetFileNameWithoutExtension(Path);
        var t_TemplateModel = new Template(t_ScriptName.ToLower(), t_ScriptName.ToLower(), "PowerShell Script", new());

        var t_TemplatesDir = ScafPaths.GetTemplatesPath();
        var t_TemplateDir = System.IO.Path.Combine(t_TemplatesDir, t_TemplateModel.Name);
        if(Directory.Exists(t_TemplateDir)) throw new Exception("Template already exists.");

        Directory.CreateDirectory(t_TemplateDir);
        File.Copy(Path, System.IO.Path.Combine(t_TemplateDir, $"{t_ScriptName}.ps1"));
        var t_TemplateJson = JsonSerializer.Serialize(t_TemplateModel, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(System.IO.Path.Combine(t_TemplateDir, "template.scaf.json"), t_TemplateJson);
    });

    public Result<Unit> UninstallTemplate(string Name) => Try.Invoke(() => {
        var t_TemplatePath = GetTemplatePath(Name);
        if(!t_TemplatePath.HasValue) throw new Exception("Template not found.");
        Directory.Delete(t_TemplatePath.Value, true);
    });

    public Maybe<string> GetTemplatePath(string TemplateName)
    {
        var t_TemplatesDir = ScafPaths.GetTemplatesPath();
        var t_TemplateDir = System.IO.Path.Combine(t_TemplatesDir, TemplateName);
        if(Directory.Exists(t_TemplateDir)) return t_TemplateDir;
        return Maybe.None;
    }
}