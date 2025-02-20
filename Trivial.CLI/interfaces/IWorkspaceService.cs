using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IWorkspaceService
{
    Result<WorkspaceConfig> Init(string Path, Maybe<string> Name);
    Maybe<WorkspaceConfig> FindWorkspace();
    Result<Unit> AddWorkspace(string Path);
    List<WorkspaceConfig> GetWorkspaces();
    Maybe<string> GetWorkspacePath(Guid Id);
}