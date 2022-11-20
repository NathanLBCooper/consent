package domain

import (
	"github.com/google/uuid"
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
