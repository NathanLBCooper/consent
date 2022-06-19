package domain

import (
	"context"

	"github.com/pkg/errors"
)

type User struct {
	Name string
}

type UserModel struct {
	Model
	User
}

type Organization struct {
	Name    string
	Members map[string]*OrganizationMember
}

type OrganizationModel struct {
	Model
	Organization
}

type OrganizationMember struct {
	Role Role
}

type Role struct {
	slug string
}

func (r Role) String() string {
	return r.slug
}

var (
	Owner = Role{"owner"}
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

func (e *AccountEndpoint) UserCreate(ctx Context, user User) (*UserModel, error) {
	return e.accountRepo.UserCreate(ctx, user)
}

func (e *AccountEndpoint) UserGet(ctx Context, id string) (*UserModel, error) {
	return e.accountRepo.UserGet(ctx, id)
}

type OrganizationCreateRequest struct {
	Name        string
	OwnerUserId string
}

func (e *AccountEndpoint) OrganizationCreate(ctx Context, req OrganizationCreateRequest) (*OrganizationModel, error) {
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

func (e *AccountEndpoint) OrganizationGet(ctx Context, id string) (*OrganizationModel, error) {
	return e.accountRepo.OrganizationGet(ctx, id)
}
