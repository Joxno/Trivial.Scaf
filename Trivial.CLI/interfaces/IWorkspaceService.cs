using Trivial.CLI.config;

namespace Trivial.CLI.interfaces;

public interface IWorkspaceService
{
    Result<WorkspaceConfig> Init(string Path, Maybe<string> Name);
    Maybe<FoundWorkspace> FindWorkspace();
    Maybe<FoundWorkspace> FindWorkspaceFromPath(string Path);
    Result<Unit> SaveWorkspace(WorkspaceConfig Workspace);
    Result<Unit> AddWorkspace(string Path);
    List<WorkspaceConfig> GetWorkspaces();
    Maybe<string> GetWorkspacePath(Guid Id);
}

public record struct FoundWorkspace(WorkspaceConfig Workspace, string Path);