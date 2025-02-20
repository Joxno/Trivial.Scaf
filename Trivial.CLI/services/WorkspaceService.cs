using Trivial.CLI.config;
using Trivial.CLI.data;
using Trivial.CLI.extensions;
using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class WorkspaceService(ISettingsService Service) : IWorkspaceService
{
    public Maybe<WorkspaceConfig> FindWorkspace() =>
        _SearchForWorkspace(Environment.CurrentDirectory)
            .Bind(P => {
                var t_WorkspacePath = System.IO.Path.Combine(P, ".scaf", "workspace.scaf.json");
                return File.Exists(t_WorkspacePath) ?
                    File.ReadAllText(t_WorkspacePath).FromJson<WorkspaceConfig>().ToMaybe() :
                    null!;
            });

    private Maybe<string> _SearchForWorkspace(Maybe<string> Path) =>
        Path.Bind(P => {
            var t_Directories = Directory.GetDirectories(P);
            var t_ScafDir = t_Directories.FirstOrNone(D => D.EndsWith(".scaf"));

            return t_ScafDir.HasValue ? t_ScafDir : 
                _SearchForWorkspace(Directory.GetParent(P).ToMaybe()
                    .Map(DI => DI!.FullName));
        });

    public Result<WorkspaceConfig> Init(string Path, Maybe<string> Name) => Try.Invoke(() => {
        var t_ResolvedPath = ScafPaths.ResolvePath(Path);
        var t_ScafDirPath = System.IO.Path.Combine(t_ResolvedPath, ".scaf");
        if(!Directory.Exists(t_ScafDirPath)) 
            Directory.CreateDirectory(t_ScafDirPath);

        var t_Workspace = new WorkspaceConfig(
            Guid.NewGuid(),
            Name.ValueOr("new-workspace"),
            new()
        );

        File.WriteAllText(System.IO.Path.Combine(t_ScafDirPath, "workspace.scaf.json"), t_Workspace.ToJson());

        return t_Workspace;
    });

    public Result<Unit> AddWorkspace(string Path) =>
        _GetWorkspaceAt(ScafPaths.ResolvePath(Path))
            .Bind(W => {
                var t_Workspace = new WorkspaceRef(W.Id, W.Name, ScafPaths.ResolvePath(Path));
                Service.SaveWorkspacesConfig(t_Workspace);
                return Result.Unit;
            });

    private Result<WorkspaceConfig> _GetWorkspaceAt(string Path) => Try.Invoke(() => {
        var t_WorkspacePath = System.IO.Path.Combine(Path, ".scaf", "workspace.scaf.json");
        if(!File.Exists(t_WorkspacePath))
            return new Exception($"Workspace not found at {t_WorkspacePath}");

        return File.ReadAllText(t_WorkspacePath).FromJson<WorkspaceConfig>();
    }).Flatten();

    public List<WorkspaceConfig> GetWorkspaces() =>
        Service.GetWorkspacesConfig()
            .Bind(Cfg => Cfg.Select(W => _GetWorkspaceAt(W.Path)).ToList().InvertResult())
            .ValueOr([]);

    public Maybe<string> GetWorkspacePath(Guid Id) =>
        Service.GetWorkspacesConfig()
            .Bind(Cfg => Cfg.FirstOrNone(W => W.Id == Id).ToResult())
            .Map(W => W.Path).ToMaybe();
}
