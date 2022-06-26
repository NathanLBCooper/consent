package domain

import (
	"context"
	"time"

	"github.com/google/uuid"
)

// Ignore versioning for first iteration

type Agreement struct {
	ContractId    ContractId
	PermissionId  PermissionId
	ParticipantId ParticipantId
	DecisionTime  time.Time
	Accepted      bool
}

type AgreementId uuid.UUID
type AgreementModel struct {
	Model[AgreementId]
	Agreement
}

type CurrentAgreementKey struct {
	ParticipantId ParticipantId
	PermissionId  PermissionId
	ContractId    ContractId
}

type AgreementEndpoint struct {
	agreementRepo AgreementRepo
}

type AgreementRepo interface {
	Create(context.Context, Agreement) (*AgreementModel, error)
	Get(context.Context, AgreementId) (*AgreementModel, error)
	GetByParticipant(context.Context, ParticipantId) ([]*AgreementModel, error)

	CurrentAgreementSet(context.Context, CurrentAgreementKey, Agreement) error
	CurrentAgreementGet(context.Context, CurrentAgreementKey) (*Agreement, bool, error)
	CurrentAgreementFilterByPP(context.Context, ParticipantId, PermissionId) ([]*Agreement, error)
	CurrentAgreementFilterByP(context.Context, ParticipantId) ([]*Agreement, error)
}

func NewAgreementEndpoint(agreementRepo AgreementRepo) (*AgreementEndpoint, error) {
	return &AgreementEndpoint{agreementRepo: agreementRepo}, nil
}

func (e *AgreementEndpoint) AgreementCreate(ctx Context, agreement Agreement) (*AgreementModel, error) {
	model, err := e.agreementRepo.Create(ctx, agreement)
	if err != nil {
		panic("todo AgreementCreate")
	}

	key := CurrentAgreementKey{ParticipantId: model.ParticipantId, PermissionId: model.PermissionId, ContractId: model.ContractId}
	current, exists, err := e.agreementRepo.CurrentAgreementGet(ctx, key)
	if err != nil {
		panic("todo CurrentAgreementTryGet")
	}

	if !exists || model.DecisionTime.After(current.DecisionTime) {
		err = e.agreementRepo.CurrentAgreementSet(ctx, key, model.Agreement)
		if err != nil {
			panic("todo CurrentAgreementSet")
		}
	}

	return model, nil
}

func (e *AgreementEndpoint) AgreementGet(ctx Context, id AgreementId) (*AgreementModel, error) {
	model, err := e.agreementRepo.Get(ctx, id)
	if err != nil {
		panic("todo AgreementGet")
	}

	return model, nil
}

func (e *AgreementEndpoint) AgreementGetByParticipant(ctx Context, participantId ParticipantId) ([]*AgreementModel, error) {
	results, err := e.agreementRepo.GetByParticipant(ctx, participantId)
	if err != nil {
		panic("todo AgreementGetByParticipant")
	}

	return results, nil
}

func (e *AgreementEndpoint) IsAccepted(ctx Context, participantId ParticipantId, permissionId PermissionId) (bool, error) {
	agreements, err := e.agreementRepo.CurrentAgreementFilterByPP(ctx, participantId, permissionId)
	if err != nil {
		panic("todo CurrentAgreementGet_1")
	}

	isAccepted := false
	for _, ag := range agreements {
		if ag.Accepted {
			isAccepted = true
			break
		}
	}

	return isAccepted, nil
}

func (e *AgreementEndpoint) GetPermissions(ctx Context, participantId ParticipantId) ([]PermissionId, error) {
	agreements, err := e.agreementRepo.CurrentAgreementFilterByP(ctx, participantId)
	if err != nil {
		panic("todo CurrentAgreementGet_2")
	}

	acceptedPermissions := []PermissionId{}
	for _, ag := range agreements {
		if ag.Accepted {
			acceptedPermissions = append(acceptedPermissions, ag.PermissionId)
		}

	}

	return acceptedPermissions, nil
}
