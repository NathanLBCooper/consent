package domain

import (
	"errors"
)

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

// todo joining of Participant in different organisation. Person? Remove OrganizationId and ExternalId from Participant?

type ParticipantEndpoint struct {
}

func NewParticipantEndpoint() (*ParticipantEndpoint, error) {
	return &ParticipantEndpoint{}, nil
}

type ParticipantCreateRequest struct {
	ExternalId     string
	OrganizationId string
}

func (e *ParticipantEndpoint) ParticipantCreate(ctx Context, req ParticipantGetRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

type ParticipantGetRequest struct {
	Id string
}

func (e *ParticipantEndpoint) ParticipantGet(ctx Context, req ParticipantGetRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

type ParticipantGetByExternalIdRequest struct {
	ExternalId string
}

func (e *ParticipantEndpoint) ParticipantGetByExternalId(ctx Context, req ParticipantGetRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}
