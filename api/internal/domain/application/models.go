package application

import "nathan.dev/consent/internal/domain"

// Some specific idea that can be agreed to
type Permission struct {
	Name string
}

type PermissionModel struct {
	domain.Model
	Permission
}

// Some touchpoint eg Registration
type Touchpoint struct {
	Name string
}

type TouchpointModel struct {
	domain.Model
	Touchpoint
}
