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
    public ProvisionId ProvisionId { get; }
    public ContractVersionId ContractVersionId { get; }
    public ContractId ContractId { get; }
    public ImmutableArray<PurposeId> PurposeIds { get; }

    public ParticipantId ParticipantId { get; }

    public DateTime DecisionTime { get; }
    public bool Accepted { get; }

    public static Agreement Ctor(ProvisionId provisionId, ContractVersionId contractVersionId,
        ContractId contractId, IEnumerable<PurposeId> purposeIds,
        ParticipantId participantId, DateTime decisionTime, bool accepted)
    {
        return new Agreement(provisionId, contractVersionId, contractId, purposeIds, participantId, decisionTime,
            accepted);
    }

    private Agreement(ProvisionId provisionId, ContractVersionId contractVersionId,
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
