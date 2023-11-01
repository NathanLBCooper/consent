using System.Threading;
using System.Threading.Tasks;
using Consent.Domain.Core;
using Consent.Domain.Workspaces;

namespace Consent.Application.Workspaces;

public interface IWorkspaceGetQueryHandler : IQueryHandler<WorkspaceGetQuery, Maybe<Workspace>> { }

public class WorkspaceGetQueryHandler : IWorkspaceGetQueryHandler
{
    private readonly IWorkspaceRepository _workspaceRepository;

    public WorkspaceGetQueryHandler(IWorkspaceRepository workspaceRepository)
    {
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Maybe<Workspace>> Handle(WorkspaceGetQuery query, CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepository.Get(query.WorkspaceId);
        if (workspace is null)
        {
            return Maybe<Workspace>.None;
        }

        if (!workspace.UserCanView(query.RequestedBy))
        {
            return Maybe<Workspace>.None;
        }

        return Maybe<Workspace>.Some(workspace);
    }
}
