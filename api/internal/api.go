package internal

import (
	"context"
	"errors"
)

type API struct {
	db      Database
	session SessionStorage
}

type Database interface {
	AccountCreate(context.Context, Account) (*AccountModel, error)
	AccountGet(context.Context, string) (*AccountModel, error)
	OrganizationCreate(context.Context, Organization) (*OrganizationModel, error)
	OrganizationGet(context.Context, string) (*OrganizationModel, error)
}

type SessionStorage interface {
	Get(sessionID string, key string) string
	Set(sessionID string, key string, value string)
}

type Context interface {
	context.Context
	ID() string
}

func NewAPI(db Database, session SessionStorage) (*API, error) {
	v := &API{db: db, session: session}
	return v, nil
}

type AccountCreateRequest struct {
	Name string
}

func (a *API) AccountCreate(ctx Context, in AccountCreateRequest) (*AccountModel, error) {
	return a.db.AccountCreate(ctx, Account{Name: in.Name})
}

type AccountGetRequest struct {
	ID string
}

func (a *API) AccountGet(ctx Context, in AccountGetRequest) (*AccountModel, error) {
	return a.db.AccountGet(ctx, in.ID)
}

type OrganizationCreateRequest struct {
	Name string
}

func (a *API) OrganizationCreate(ctx Context, in OrganizationCreateRequest) (*OrganizationModel, error) {
	id := a.session.Get(ctx.ID(), "account")
	if id == "" {
		return nil, errors.New("not_logged_in")
	}
	return a.db.OrganizationCreate(ctx, Organization{
		Name: in.Name,
		Members: map[string]*OrganizationMember{
			id: {Role: "owner"},
		},
	})
}

type OrganizationGetRequest struct {
	ID string
}

func (a *API) OrganizationGet(ctx Context, in OrganizationGetRequest) (*OrganizationModel, error) {
	return a.db.OrganizationGet(ctx, in.ID)
}
