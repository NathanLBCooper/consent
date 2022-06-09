package user

import (
	"github.com/pkg/errors"
	"nathan.dev/consent/internal/domain"
)

type Endpoint struct {
	repo Repo
}

type AccountCreateRequest struct {
	Name string
}

func NewEndpoint(repo Repo) (*Endpoint, error) {
	return &Endpoint{repo: repo}, nil
}

func (e *Endpoint) CreateAccount(ctx domain.Context, req AccountCreateRequest) (*AccountModel, error) {
	return e.repo.CreateAccount(ctx, Account{Name: req.Name})
}

type AccountGetRequest struct {
	Id string
}

func (e *Endpoint) GetAccount(ctx domain.Context, req AccountGetRequest) (*AccountModel, error) {
	return e.repo.GetAccount(ctx, req.Id)
}

type OrganizationCreateRequest struct {
	Name           string
	OwnerAccountId string
}

func (e *Endpoint) CreateOrganization(ctx domain.Context, req OrganizationCreateRequest) (*OrganizationModel, error) {
	member, err := e.repo.GetAccount(ctx, req.OwnerAccountId)
	if err != nil {
		return nil, errors.Wrap(err, "Owning account")
	}

	return e.repo.CreateOrganization(ctx, Organization{
		Name: req.Name,
		Members: map[string]*OrganizationMember{
			member.Id: {Role: Owner},
		},
	})
}

type OrganizationGetRequest struct {
	Id string
}

func (a *Endpoint) GetOrganization(ctx domain.Context, req OrganizationGetRequest) (*OrganizationModel, error) {
	return a.repo.GetOrganization(ctx, req.Id)
}
