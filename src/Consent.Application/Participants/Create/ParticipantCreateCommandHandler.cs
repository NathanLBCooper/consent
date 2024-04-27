using System.Threading;
using System.Threading.Tasks;
using Consent.Application.Workspaces;
using Consent.Domain.Participants;

namespace Consent.Application.Participants.Create;

public interface IParticipantCreateCommandHandler : ICommandHandler<ParticipantCreateCommand, Participant> { }

public class ParticipantCreateCommandHandler : IParticipantCreateCommandHandler
{
    private readonly IParticipantRepository _participantRepository;
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly ParticipantCreateCommandValidator _validator = new();

    public ParticipantCreateCommandHandler(IParticipantRepository participantRepository, IWorkspaceRepository workspaceRepository)
    {
        _participantRepository = participantRepository;
        _workspaceRepository = workspaceRepository;
    }

    public async Task<Participant> Handle(ParticipantCreateCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        throw new System.NotImplementedException();
    }
}
