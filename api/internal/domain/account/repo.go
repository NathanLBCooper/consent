package account

import (
	"context"
)

type Repo interface {
	CreateUser(context.Context, User) (*UserModel, error)
	GetUser(context.Context, string) (*UserModel, error)
	CreateOrganization(context.Context, Organization) (*OrganizationModel, error)
	GetOrganization(context.Context, string) (*OrganizationModel, error)
}
