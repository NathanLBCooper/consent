package domain

import (
	"context"
)

type ParticipantRepo interface {
	ParticipantCreate(context.Context, Participant) (*ParticipantModel, error)
	ParticipantGet(context.Context, ParticipantId) (*ParticipantModel, error)
	ParticipantGetByExternalId(context.Context, ParticipantExternalId) (*ParticipantModel, error)
}

type ParticipantEndpoint struct {
	participantRepo ParticipantRepo
}

func NewParticipantEndpoint(participantRepo ParticipantRepo) (*ParticipantEndpoint, error) {
	return &ParticipantEndpoint{participantRepo: participantRepo}, nil
}

func (e *ParticipantEndpoint) ParticipantCreate(ctx Context, participant Participant) (*ParticipantModel, error) {
	return e.participantRepo.ParticipantCreate(ctx, participant)
}

func (e *ParticipantEndpoint) ParticipantGet(ctx Context, id ParticipantId) (*ParticipantModel, error) {
	return e.participantRepo.ParticipantGet(ctx, id)
}

func (e *ParticipantEndpoint) ParticipantGetByExternalId(ctx Context, externalId ParticipantExternalId) (*ParticipantModel, error) {
	return e.participantRepo.ParticipantGetByExternalId(ctx, externalId)
}
