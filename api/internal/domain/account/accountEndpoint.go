package account

import (
	"context"

	"github.com/pkg/errors"
	"nathan.dev/consent/internal/domain"
)

type AccountEndpoint struct {
	accountRepo AccountRepo
}

type AccountRepo interface {
	UserCreate(context.Context, User) (*UserModel, error)
	UserGet(context.Context, string) (*UserModel, error)
	OrganizationCreate(context.Context, Organization) (*OrganizationModel, error)
	OrganizationGet(context.Context, string) (*OrganizationModel, error)
}

func NewAccountEndpoint(accountRepo AccountRepo) (*AccountEndpoint, error) {
	return &AccountEndpoint{accountRepo: accountRepo}, nil
}

type UserCreateRequest struct {
	Name string
}

func (e *AccountEndpoint) UserCreate(ctx domain.Context, req UserCreateRequest) (*UserModel, error) {
	return e.accountRepo.UserCreate(ctx, User{Name: req.Name})
}

type UserGetRequest struct {
	Id string
}

func (e *AccountEndpoint) UserGet(ctx domain.Context, req UserGetRequest) (*UserModel, error) {
	return e.accountRepo.UserGet(ctx, req.Id)
}

type OrganizationCreateRequest struct {
	Name        string
	OwnerUserId string
}

func (e *AccountEndpoint) OrganizationCreate(ctx domain.Context, req OrganizationCreateRequest) (*OrganizationModel, error) {
	member, err := e.accountRepo.UserGet(ctx, req.OwnerUserId)
	if err != nil {
		return nil, errors.Wrap(err, "Owning user")
	}

	return e.accountRepo.OrganizationCreate(ctx, Organization{
		Name: req.Name,
		Members: map[string]*OrganizationMember{
			member.Id: {Role: Owner},
		},
	})
}

type OrganizationGetRequest struct {
	Id string
}

func (e *AccountEndpoint) OrganizationGet(ctx domain.Context, req OrganizationGetRequest) (*OrganizationModel, error) {
	return e.accountRepo.OrganizationGet(ctx, req.Id)
}
