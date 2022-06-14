package participant

import (
	"nathan.dev/consent/internal/domain"
)

type Participant struct {
	ExternalId            string
	OrganizationId        string
	AcceptedPermissionIds []string // read model. Calculated from AllAgreements
	AllAgreements         []*Agreement
}

type ParticipantModel struct {
	domain.Model
	Participant
}

// todo joining of Participant in different organisation. Person? Remove OrganizationId and ExternalId from Participant?
