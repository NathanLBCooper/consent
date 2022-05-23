package internal

import (
	"time"
)

type Model struct {
	ID      string
	Created time.Time
	Updated time.Time
	Removed bool
}

type Account struct {
	Name string
}

type AccountModel struct {
	Model
	Account
}

type Organization struct {
	Name    string
	Members map[string]*OrganizationMember
}

type OrganizationMember struct {
	Role string
}

type OrganizationModel struct {
	Model
	Organization
}
