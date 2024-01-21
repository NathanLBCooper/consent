using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Core;
using Consent.Domain.Participants;

namespace Consent.Application.Participants.Get;

public interface IParticipantGetQueryHandler : IQueryHandler<ParticipantGetQuery, Maybe<Participant>> { }

public class ParticipantGetQueryHandler : IParticipantGetQueryHandler
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IWorkspaceRepository _workspaceRepository;

    public ParticipantGetQueryHandler(IParticipantRepository participantRepository, IWorkspaceRepository workspaceRepository)
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Maybe<Participant>> Handle(ParticipantGetQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        throw new System.NotImplementedException();
    }
}
