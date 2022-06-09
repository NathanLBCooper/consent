package testcommon

import (
	"context"
	"errors"
	"time"

	"github.com/google/uuid"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/domain/user"
)

type FakeUserRepo struct {
	accounts      []*user.AccountModel
	organizations []*user.OrganizationModel
}

var _ user.Repo = &FakeUserRepo{}

func (r *FakeUserRepo) CreateAccount(ctx context.Context, v user.Account) (*user.AccountModel, error) {
	now := time.Now()
	model := &user.AccountModel{
		Model:   domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Account: v,
	}

	r.accounts = append(r.accounts, model)
	return model, nil
}

func (r *FakeUserRepo) GetAccount(ctx context.Context, id string) (*user.AccountModel, error) {
	for i := range r.accounts {
		if r.accounts[i].Id == id {
			return r.accounts[i], nil
		}
	}

	return nil, errors.New("not found")
}

func (r *FakeUserRepo) CreateOrganization(ctx context.Context, v user.Organization) (*user.OrganizationModel, error) {
	now := time.Now()
	model := &user.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: v,
	}

	r.organizations = append(r.organizations, model)
	return model, nil
}

func (r *FakeUserRepo) GetOrganization(ctx context.Context, id string) (*user.OrganizationModel, error) {
	for i := range r.organizations {
		if r.organizations[i].Id == id {
			return r.organizations[i], nil
		}
	}

	return nil, errors.New("not found")
}
