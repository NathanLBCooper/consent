using System;
using Consent.Domain.Contracts;
using Consent.Domain.Participants;
using Consent.Domain.Permissions;

namespace Consent.Domain.Agreements;

/*
 * The response of a Participant to a Provision
 * For a given participant, the sum total of these Agreements describes and audits what Permissions they have accepted
 */

public record Agreement
{
    public ProvisionId ProvisionId { get; }
    public ContractVersionId ContractVersionId { get; }
    public ContractId ContractId { get; }
    public PermissionId[] PermissionsIds { get; }

    public ParticipantId ParticipantId { get; }

    public DateTime DecisionTime { get; }
    public bool Accepted { get; }

    public Agreement(ProvisionId provisionId, ContractVersionId contractVersionId,
        ContractId contractId, PermissionId[] permissionsIds,
        ParticipantId participantId, DateTime decisionTime, bool accepted)
    {
        ProvisionId = provisionId;
        ContractVersionId = contractVersionId;
        ContractId = contractId;
        PermissionsIds = permissionsIds;
        ParticipantId = participantId;
        DecisionTime = decisionTime;
        Accepted = accepted;
    }
}
