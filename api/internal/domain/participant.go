package domain

import (
	"go.mongodb.org/mongo-driver/x/mongo/driver/uuid"
)

// Participants just belong to one organisation for now.

type Participant struct {
	ExternalId     ParticipantExternalId
	OrganizationId OrganizationId
	Tags           []Tag
}

type ParticipantId uuid.UUID
type ParticipantModel struct {
	Model[ParticipantId]
	Participant
}

type ParticipantExternalId string
