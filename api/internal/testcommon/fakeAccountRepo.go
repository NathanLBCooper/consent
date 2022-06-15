package testcommon

import (
	"context"
	"errors"
	"time"

	"github.com/google/uuid"
	"nathan.dev/consent/internal/domain"
)

type FakeAccountRepo struct {
	users         []*domain.UserModel
	organizations []*domain.OrganizationModel
}

var _ domain.AccountRepo = &FakeAccountRepo{}

func (r *FakeAccountRepo) UserCreate(ctx context.Context, user domain.User) (*domain.UserModel, error) {
	now := time.Now()
	model := &domain.UserModel{
		Model: domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		User:  user,
	}

	r.users = append(r.users, model)
	return model, nil
}

func (r *FakeAccountRepo) UserGet(ctx context.Context, id string) (*domain.UserModel, error) {
	for i := range r.users {
		if r.users[i].Id == id {
			return r.users[i], nil
		}
	}

	return nil, errors.New("not found")
}

func (r *FakeAccountRepo) OrganizationCreate(ctx context.Context, org domain.Organization) (*domain.OrganizationModel, error) {
	now := time.Now()
	model := &domain.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: org,
	}

	r.organizations = append(r.organizations, model)
	return model, nil
}

func (r *FakeAccountRepo) OrganizationGet(ctx context.Context, id string) (*domain.OrganizationModel, error) {
	for i := range r.organizations {
		if r.organizations[i].Id == id {
			return r.organizations[i], nil
		}
	}

	return nil, errors.New("not found")
}
