using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Consent.Domain.Contracts;
using Consent.Domain.Participants;
using Consent.Domain.Purposes;

namespace Consent.Domain.Agreements;

/*
 * The response of a Participant to a Provision
 * For a given participant, the sum total of these Agreements describes and audits what Purposes they have accepted
 */

public class Agreement
{
    public ProvisionId ProvisionId { get; private init; }
    public ContractVersionId ContractVersionId { get; private init; }
    public ContractId ContractId { get; private init; }
    public ImmutableArray<PurposeId> PurposeIds { get; private init; }

    public ParticipantId ParticipantId { get; private init; }

    public DateTime DecisionTime { get; private init; }
    public bool Accepted { get; private init; }

    public Agreement(ProvisionId provisionId, ContractVersionId contractVersionId,
        ContractId contractId, IEnumerable<PurposeId> purposeIds,
        ParticipantId participantId, DateTime decisionTime, bool accepted)
    {
        ProvisionId = provisionId;
        ContractVersionId = contractVersionId;
        ContractId = contractId;
        PurposeIds = purposeIds.ToImmutableArray();
        ParticipantId = participantId;
        DecisionTime = decisionTime;
        Accepted = accepted;
    }
}
