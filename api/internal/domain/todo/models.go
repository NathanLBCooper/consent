package todo

import "nathan.dev/consent/internal/domain"

type Permission struct {
	Name string
}

type PermissionModel struct {
	domain.Model
	Permission
}

type Provision struct {
	PermissionsIds []string
}
