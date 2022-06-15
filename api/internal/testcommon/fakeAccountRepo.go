package testcommon

import (
	"context"
	"errors"
	"time"

	"github.com/google/uuid"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/domain/account"
)

type FakeAccountRepo struct {
	users         []*account.UserModel
	organizations []*account.OrganizationModel
}

var _ account.AccountRepo = &FakeAccountRepo{}

func (r *FakeAccountRepo) UserCreate(ctx context.Context, user account.User) (*account.UserModel, error) {
	now := time.Now()
	model := &account.UserModel{
		Model: domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		User:  user,
	}

	r.users = append(r.users, model)
	return model, nil
}

func (r *FakeAccountRepo) UserGet(ctx context.Context, id string) (*account.UserModel, error) {
	for i := range r.users {
		if r.users[i].Id == id {
			return r.users[i], nil
		}
	}

	return nil, errors.New("not found")
}

func (r *FakeAccountRepo) OrganizationCreate(ctx context.Context, org account.Organization) (*account.OrganizationModel, error) {
	now := time.Now()
	model := &account.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: org,
	}

	r.organizations = append(r.organizations, model)
	return model, nil
}

func (r *FakeAccountRepo) OrganizationGet(ctx context.Context, id string) (*account.OrganizationModel, error) {
	for i := range r.organizations {
		if r.organizations[i].Id == id {
			return r.organizations[i], nil
		}
	}

	return nil, errors.New("not found")
}
