package domain

import (
	"errors"
	"time"
)

type Agreement struct {
	ParticipantId     string
	ContractVersionId string
	PermissionId      string
	AcceptedTime      time.Time
	// todo more optional context. Document, Touchpoint etc?
}

type Acceptance struct {
	Agreement
	Accepted bool
}

type AcceptanceModel struct {
	Acceptance
	Model
}

type AcceptanceEndpoint struct {
}

func NewAcceptanceEndpoint() (*AcceptanceEndpoint, error) {
	return &AcceptanceEndpoint{}, nil
}

func (e *AcceptanceEndpoint) AcceptanceCreate(ctx Context, req Acceptance) (*Agreement, error) {
	return nil, errors.New("not implemented")
}

func (e *AcceptanceEndpoint) IsAccepted(ctx Context, permissionId string) (bool, *AcceptanceModel, error) {
	return false, nil, errors.New("not implemented")
}

func (e *AcceptanceEndpoint) GetAcceptedPermissions(ctx Context, participantId string) ([]string, []*AcceptanceModel, error) {
	return nil, nil, errors.New("not implemented")
}
