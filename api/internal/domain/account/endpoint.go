package account

import (
	"github.com/pkg/errors"
	"nathan.dev/consent/internal/domain"
)

type Endpoint struct {
	repo Repo
}

type UserCreateRequest struct {
	Name string
}

func NewEndpoint(repo Repo) (*Endpoint, error) {
	return &Endpoint{repo: repo}, nil
}

func (e *Endpoint) CreateUser(ctx domain.Context, req UserCreateRequest) (*UserModel, error) {
	return e.repo.CreateUser(ctx, User{Name: req.Name})
}

type UserGetRequest struct {
	Id string
}

func (e *Endpoint) GetUser(ctx domain.Context, req UserGetRequest) (*UserModel, error) {
	return e.repo.GetUser(ctx, req.Id)
}

type OrganizationCreateRequest struct {
	Name        string
	OwnerUserId string
}

func (e *Endpoint) CreateOrganization(ctx domain.Context, req OrganizationCreateRequest) (*OrganizationModel, error) {
	member, err := e.repo.GetUser(ctx, req.OwnerUserId)
	if err != nil {
		return nil, errors.Wrap(err, "Owning user")
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
