package domain

import "time"

type Participant struct {
	ExternalId            string
	OrganizationId        string
	AcceptedPermissionIds []string // read model. Calculated from AllAgreements
	AllAgreements         []*Agreement
}

type ParticipantModel struct {
	Model
	Participant
}

type Agreement struct {
	ContractVersionId string
	PermissionId      string
	Accepted          bool
	AcceptedTime      time.Time
	// todo more optional context. Document, Touchpoint etc?
}

// todo joining of Participant in different organisation. Person? Remove OrganizationId and ExternalId from Participant?
