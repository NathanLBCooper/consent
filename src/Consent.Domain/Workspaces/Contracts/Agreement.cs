using System;
using Consent.Domain.Participants;
using Consent.Domain.Workspaces.Permissions;

namespace Consent.Domain.Workspaces.Contracts;

/*
 * The response of a Participant to a Provision
 * For a given participant, the sum total of these Agreements describes and audits what Permissions they have accepted
 */

public record Agreement
{
    // todo ProvisionId
    public ContractId ContractId { get; private init; }
    public PermissionId PermissionsId { get; private init; }
    public ParticipantId ParticipantId { get; private init; }
    public DateTime DecisionTime { get; private init; }
    public bool Accepted { get; private init; }
}
