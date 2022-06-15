package domain

import (
	"errors"
)

type ParticipantEndpoint struct {
	//participantRepo ParticipantRepo
}

// type ParticipantRepo interface {
// }

func NewParticipantEndpoint() (*ParticipantEndpoint, error) { // participantRepo ParticipantRepo
	return &ParticipantEndpoint{}, nil // participantRepo: participantRepo
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

type AcceptedPermissionsGetRequest struct {
	ParticipantId string
}

type AcceptedPermissionsGetResponse struct {
	ParticipantId       string
	AcceptedPermissions *PermissionModel
}

func (e *ParticipantEndpoint) AcceptedPermissionsGet(ctx Context, req AcceptedPermissionsGetRequest) (*AcceptedPermissionsGetResponse, error) {
	return nil, errors.New("not implemented")
}
