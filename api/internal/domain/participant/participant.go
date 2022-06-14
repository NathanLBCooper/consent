package participant

import (
	"nathan.dev/consent/internal/domain"
)

type Participant struct {
	ExternalId         string
	OrganizationId     string
	ActiveAgreementIds []string // read model, agreements with those "overwritten" by a fresher responses to a SectionVersion filtered out
	AllAgreementsIds   []string
}

type ParticipantModel struct {
	domain.Model
	Participant
}

// todo joining of Participant in different organisation. Person? Remove OrganizationId and ExternalId from Participant?
