package domain

import (
	"context"

	"github.com/google/uuid"
	"github.com/pkg/errors"
)

type User struct {
	Name string
}

type UserId uuid.UUID
type UserModel struct {
	Model[UserId]
	User
}

type Organization struct {
	Name    string
	Members map[UserId]*OrganizationMember
}

type OrganizationId uuid.UUID
type OrganizationModel struct {
	Model[OrganizationId]
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

type AccountRepo interface {
	UserCreate(context.Context, User) (*UserModel, error)
	UserGet(context.Context, UserId) (*UserModel, error)
	OrganizationCreate(context.Context, Organization) (*OrganizationModel, error)
	OrganizationGet(context.Context, OrganizationId) (*OrganizationModel, error)
}

type OrganizationCreateRequest struct {
	Name        string
	OwnerUserId UserId
}

type AccountEndpoint struct {
	accountRepo AccountRepo
}

func NewAccountEndpoint(accountRepo AccountRepo) (*AccountEndpoint, error) {
	return &AccountEndpoint{accountRepo: accountRepo}, nil
}

func (e *AccountEndpoint) UserCreate(ctx Context, user User) (*UserModel, error) {
	return e.accountRepo.UserCreate(ctx, user)
}

func (e *AccountEndpoint) UserGet(ctx Context, id UserId) (*UserModel, error) {
	return e.accountRepo.UserGet(ctx, id)
}

func (e *AccountEndpoint) OrganizationCreate(ctx Context, req OrganizationCreateRequest) (*OrganizationModel, error) {
	member, err := e.accountRepo.UserGet(ctx, req.OwnerUserId)
	if err != nil {
		return nil, errors.Wrap(err, "Owning user")
	}

	return e.accountRepo.OrganizationCreate(ctx, Organization{
		Name: req.Name,
		Members: map[UserId]*OrganizationMember{
			member.Id: {Role: Owner},
		},
	})
}

func (e *AccountEndpoint) OrganizationGet(ctx Context, id OrganizationId) (*OrganizationModel, error) {
	return e.accountRepo.OrganizationGet(ctx, id)
}
