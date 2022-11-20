package domain

import (
	"github.com/google/uuid"
)

// A specific idea that can be agreed to
// Eg: 'Receive information and offers', 'Use of cookies', 'Share your knäckebröd preferences with Wasa"
type Permission struct {
	Name           string
	OrganizationId string
}

type PermissionId uuid.UUID
type PermissionModel struct {
	Model[PermissionId]
	Permission
}
