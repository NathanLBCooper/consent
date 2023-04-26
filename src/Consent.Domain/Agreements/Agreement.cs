using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Consent.Domain.Contracts;
using Consent.Domain.Participants;
using Consent.Domain.Permissions;

namespace Consent.Domain.Agreements;

/*
 * The response of a Participant to a Provision
 * For a given participant, the sum total of these Agreements describes and audits what Permissions they have accepted
 */

public class Agreement
{
    public ProvisionId ProvisionId { get; private init; }
    public ContractVersionId ContractVersionId { get; private init; }
    public ContractId ContractId { get; private init; }
    public ImmutableArray<PermissionId> PermissionsIds { get; private init; }

    public ParticipantId ParticipantId { get; private init; }

    public DateTime DecisionTime { get; private init; }
    public bool Accepted { get; private init; }

    public Agreement(ProvisionId provisionId, ContractVersionId contractVersionId,
        ContractId contractId, IEnumerable<PermissionId> permissionsIds,
        ParticipantId participantId, DateTime decisionTime, bool accepted)
    {
        ProvisionId = provisionId;
        ContractVersionId = contractVersionId;
        ContractId = contractId;
        PermissionsIds = permissionsIds.ToImmutableArray();
        ParticipantId = participantId;
        DecisionTime = decisionTime;
        Accepted = accepted;
    }
}
