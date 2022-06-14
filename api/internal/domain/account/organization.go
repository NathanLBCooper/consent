package account

import "nathan.dev/consent/internal/domain"

type Organization struct {
	Name    string
	Members map[string]*OrganizationMember
}

type OrganizationModel struct {
	domain.Model
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
