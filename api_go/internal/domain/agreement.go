package domain

import (
	"time"

	"github.com/google/uuid"
)

// Ignore versioning for first iteration

type Agreement struct {
	ContractId    ContractId
	PermissionId  PermissionId
	ParticipantId ParticipantId
	DecisionTime  time.Time
	Accepted      bool
}

type AgreementId uuid.UUID
type AgreementModel struct {
	Model[AgreementId]
	Agreement
}

type CurrentAgreementKey struct {
	ParticipantId ParticipantId
	PermissionId  PermissionId
	ContractId    ContractId
}
