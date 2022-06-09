package user

import (
	"context"
)

type Repo interface {
	CreateAccount(context.Context, Account) (*AccountModel, error)
	GetAccount(context.Context, string) (*AccountModel, error)
	CreateOrganization(context.Context, Organization) (*OrganizationModel, error)
	GetOrganization(context.Context, string) (*OrganizationModel, error)
}
