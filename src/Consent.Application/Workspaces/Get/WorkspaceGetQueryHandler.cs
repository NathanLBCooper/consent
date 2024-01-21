using System.Threading;
using System.Threading.Tasks;
using Consent.Domain.Core;
using Consent.Domain.Workspaces;
using static Consent.Domain.Core.Maybe<Consent.Domain.Workspaces.Workspace>;

namespace Consent.Application.Workspaces.Get;

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
            return None;
        }

        if (!workspace.UserCanView(query.RequestedBy))
        {
            return None;
        }

        return Some(workspace);
    }
}
