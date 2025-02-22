using Trivial.CLI.interfaces;

namespace Trivial.CLI.services;

public class ContextService(IWorkspaceService WorkspaceService) : IContextService
{
    public Result<string> ConstructContextInjection() =>
        string.Join(";", new List<Maybe<string>>() 
        {
            _CreateWorkspaceInjection()
        }.ResolveMaybes());

    private Maybe<string> _CreateWorkspaceInjection()
    {
        var t_Workspace = WorkspaceService.FindWorkspace();
        if(!t_Workspace.HasValue) return Maybe.None;

        return string.Join(";", [
            $"$ScafWorkspacePath = '{t_Workspace.Value.Path}'",
            $"$ScafWorkspaceConfigPath = '{Path.Combine(t_Workspace.Value.Path, "workspace.scaf.json")}'",
            $"$ScafWorkspace = (Get-Content {Path.Combine(t_Workspace.Value.Path, "workspace.scaf.json")} | ConvertFrom-Json -Depth 99)"
        ]);
    }
}