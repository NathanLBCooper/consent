package domain

import (
	"errors"
	"time"

	"github.com/google/uuid"
)

// Ignore versioning for first iteration

type Agreement struct {
	//ContractVersionId uuid.UUID
	ContractId    uuid.UUID
	PermissionId  uuid.UUID
	ParticipantId uuid.UUID
	DecisionTime  time.Time
	Accepted      bool
}

type AgreementModel struct {
	Agreement
	Model
}

type AgreementEndpoint struct {
}

func NewAgreementEndpoint() (*AgreementEndpoint, error) {
	return &AgreementEndpoint{}, nil
}

func (e *AgreementEndpoint) AgreementCreate(ctx Context, agreement Agreement) (*AgreementModel, error) {
	return nil, errors.New("not implemented")
}

func (e *AgreementEndpoint) AgreementGet(ctx Context, id string) (*AgreementModel, error) {
	return nil, errors.New("not implemented")
}

func (e *AgreementEndpoint) AgreementGetAll(ctx Context, participantId uuid.UUID) ([]*AgreementModel, error) {
	return nil, errors.New("not implemented")
}

func (e *AgreementEndpoint) IsAccepted(ctx Context, participantId uuid.UUID, permissionId uuid.UUID) (bool, error) {
	return false, errors.New("not implemented")
}

func (e *AgreementEndpoint) GetPermissions(ctx Context, participantId uuid.UUID) ([]string, error) {
	return nil, errors.New("not implemented")
}
