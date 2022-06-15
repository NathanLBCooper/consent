package participant

import (
	"errors"

	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/domain/application"
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

func (e *ParticipantEndpoint) ParticipantCreate(ctx domain.Context, req ParticipantGetRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

type ParticipantGetRequest struct {
	Id string
}

func (e *ParticipantEndpoint) ParticipantGet(ctx domain.Context, req ParticipantGetRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

type ParticipantGetByExternalIdRequest struct {
	ExternalId string
}

func (e *ParticipantEndpoint) ParticipantGetByExternalId(ctx domain.Context, req ParticipantGetRequest) (*ParticipantModel, error) {
	return nil, errors.New("not implemented")
}

type AcceptedPermissionsGetRequest struct {
	ParticipantId string
}

type AcceptedPermissionsGetResponse struct {
	ParticipantId       string
	AcceptedPermissions *application.PermissionModel
}

func (e *ParticipantEndpoint) AcceptedPermissionsGet(ctx domain.Context, req AcceptedPermissionsGetRequest) (*AcceptedPermissionsGetResponse, error) {
	return nil, errors.New("not implemented")
}
