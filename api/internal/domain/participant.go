package domain

import (
	"errors"

	"go.mongodb.org/mongo-driver/x/mongo/driver/uuid"
)

type Participant struct {
	ExternalId     string
	OrganizationId string
	Tags           []Tag
}

type ParticipantId uuid.UUID
type ParticipantModel struct {
	Model[ParticipantId]
	Participant
}

// todo joining of Participant in different organisation. Person? Remove OrganizationId and ExternalId from Participant?

type ParticipantCreateRequest struct {
	ExternalId     string
	OrganizationId OrganizationId
}

type ParticipantEndpoint struct {
}

func NewParticipantEndpoint() (*ParticipantEndpoint, error) {
	return &ParticipantEndpoint{}, nil
}

func (e *ParticipantEndpoint) ParticipantCreate(ctx Context, req ParticipantCreateRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

func (e *ParticipantEndpoint) ParticipantGet(ctx Context, id ParticipantId) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

func (e *ParticipantEndpoint) ParticipantGetByExternalId(ctx Context, externalId string) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}
