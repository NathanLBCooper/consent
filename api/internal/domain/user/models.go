package user

import "nathan.dev/consent/internal/domain"

type Account struct {
	Name string
}

type AccountModel struct {
	domain.Model
	Account
}

type Organization struct {
	Name    string
	Members map[string]*OrganizationMember
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

type OrganizationModel struct {
	domain.Model
	Organization
}
