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

var _ account.Repo = &FakeAccountRepo{}

func (r *FakeAccountRepo) CreateUser(ctx context.Context, v account.User) (*account.UserModel, error) {
	now := time.Now()
	model := &account.UserModel{
		Model: domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		User:  v,
	}

	r.users = append(r.users, model)
	return model, nil
}

func (r *FakeAccountRepo) GetUser(ctx context.Context, id string) (*account.UserModel, error) {
	for i := range r.users {
		if r.users[i].Id == id {
			return r.users[i], nil
		}
	}

	return nil, errors.New("not found")
}

func (r *FakeAccountRepo) CreateOrganization(ctx context.Context, v account.Organization) (*account.OrganizationModel, error) {
	now := time.Now()
	model := &account.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: v,
	}

	r.organizations = append(r.organizations, model)
	return model, nil
}

func (r *FakeAccountRepo) GetOrganization(ctx context.Context, id string) (*account.OrganizationModel, error) {
	for i := range r.organizations {
		if r.organizations[i].Id == id {
			return r.organizations[i], nil
		}
	}

	return nil, errors.New("not found")
}
