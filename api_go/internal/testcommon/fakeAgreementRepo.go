package testcommon

import (
	"context"
	"errors"
	"time"

	"github.com/google/uuid"
	"nathan.dev/consent/internal/domain"
)

type FakeAgreementRepo struct {
	agreements []*domain.AgreementModel
	current    map[domain.CurrentAgreementKey]currentAgreementValue
}

func NewFakeAgreementRepo() *FakeAgreementRepo {
	return &FakeAgreementRepo{current: map[domain.CurrentAgreementKey]currentAgreementValue{}}
}

var _ domain.AgreementRepo = &FakeAgreementRepo{}

type currentAgreementValue struct {
	DecisionTime time.Time
	Accepted     bool
}

func toAgreement(key domain.CurrentAgreementKey, value currentAgreementValue) *domain.Agreement {
	return &domain.Agreement{
		ContractId:    key.ContractId,
		PermissionId:  key.PermissionId,
		ParticipantId: key.ParticipantId,
		DecisionTime:  value.DecisionTime,
		Accepted:      value.Accepted,
	}
}

func (r *FakeAgreementRepo) Create(ctx context.Context, agreement domain.Agreement) (*domain.AgreementModel, error) {
	now := time.Now()
	model := &domain.AgreementModel{
		Model:     domain.Model[domain.AgreementId]{Id: domain.AgreementId(uuid.New()), Created: now, Updated: now},
		Agreement: agreement,
	}

	r.agreements = append(r.agreements, model)
	return model, nil
}

func (r *FakeAgreementRepo) Get(ctx context.Context, id domain.AgreementId) (*domain.AgreementModel, error) {
	for _, ag := range r.agreements {
		if ag.Id == id {
			return ag, nil
		}
	}

	return nil, errors.New("not found")
}

func (r *FakeAgreementRepo) GetByParticipant(ctx context.Context, participantId domain.ParticipantId) (
	[]*domain.AgreementModel, error) {
	participantAgreements := []*domain.AgreementModel{}
	for _, ag := range r.agreements {
		if ag.ParticipantId == participantId {
			participantAgreements = append(participantAgreements, ag)
		}
	}

	return participantAgreements, nil
}

func (r *FakeAgreementRepo) CurrentAgreementSet(ctx context.Context, key domain.CurrentAgreementKey, value domain.Agreement) error {
	r.current[key] = currentAgreementValue{DecisionTime: value.DecisionTime, Accepted: value.Accepted}
	return nil
}

func (r *FakeAgreementRepo) CurrentAgreementGet(ctx context.Context, key domain.CurrentAgreementKey) (*domain.Agreement, bool, error) {
	result, exists := r.current[key]
	if !exists {
		return nil, false, nil
	}

	return toAgreement(key, result), true, nil
}

// todo return key as well, or just and agreement?
func (r *FakeAgreementRepo) CurrentAgreementFilterByPP(ctx context.Context,
	participantId domain.ParticipantId, permissionId domain.PermissionId) ([]*domain.Agreement, error) {

	values := []*domain.Agreement{}
	for key, value := range r.current {
		if key.ParticipantId == participantId && key.PermissionId == permissionId {
			values = append(values, toAgreement(key, value))
		}
	}

	return values, nil
}

func (r *FakeAgreementRepo) CurrentAgreementFilterByP(ctx context.Context, participantId domain.ParticipantId) ([]*domain.Agreement, error) {

	values := []*domain.Agreement{}
	for key, value := range r.current {
		if key.ParticipantId == participantId {
			values = append(values, toAgreement(key, value))
		}
	}

	return values, nil
}
