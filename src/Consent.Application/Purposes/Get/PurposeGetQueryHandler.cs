using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Core;
using Consent.Domain.Purposes;
using static Consent.Domain.Core.Maybe<Consent.Domain.Purposes.Purpose>;

namespace Consent.Application.Purposes.Get;

public interface IPurposeGetQueryHandler : IQueryHandler<PurposeGetQuery, Maybe<Purpose>> { }

public class PurposeGetQueryHandler : IPurposeGetQueryHandler
{
    private readonly IPurposeRepository _purposeRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public PurposeGetQueryHandler(IPurposeRepository purposeRepository, IWorkspaceRepository workspaceRepository)
    {
        _purposeRepository = purposeRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Maybe<Purpose>> Handle(PurposeGetQuery query, CancellationToken cancellationToken)
    {
        var purpose = await _purposeRepository.Get(query.PurposeId);
        if (purpose is null)
        {
            return None;
        }

        var workspace = Guard.NotNull(await _workspaceRepository.Get(purpose.WorkspaceId));
        if (!workspace.UserCanView(query.RequestedBy))
        {
            return None;
        }

        return Some(purpose);
    }
}
