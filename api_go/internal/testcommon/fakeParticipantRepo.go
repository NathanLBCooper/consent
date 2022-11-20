package testcommon

import (
	"context"
	"errors"
	"time"

	"github.com/google/uuid"
	"nathan.dev/consent/internal/domain"
)

type FakeParticipantRepo struct {
	participants []*domain.ParticipantModel
}

var _ domain.ParticipantRepo = &FakeParticipantRepo{}

func (r *FakeParticipantRepo) ParticipantCreate(ctx context.Context, participant domain.Participant) (*domain.ParticipantModel, error) {
	now := time.Now()
	model := &domain.ParticipantModel{
		Model:       domain.Model[domain.ParticipantId]{Id: domain.ParticipantId(uuid.New()), Created: now, Updated: now},
		Participant: participant,
	}

	r.participants = append(r.participants, model)
	return model, nil
}

func (r *FakeParticipantRepo) ParticipantGet(ctx context.Context, id domain.ParticipantId) (*domain.ParticipantModel, error) {
	for i := range r.participants {
		if r.participants[i].Id == id {
			return r.participants[i], nil
		}
	}

	return nil, errors.New("not found")
}

func (r *FakeParticipantRepo) ParticipantGetByExternalId(ctx context.Context, externalId domain.ParticipantExternalId) (*domain.ParticipantModel, error) {
	for i := range r.participants {
		if r.participants[i].ExternalId == externalId {
			return r.participants[i], nil
		}
	}

	return nil, errors.New("not found")
}
