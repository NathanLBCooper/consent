package domain

import (
	"errors"
)

type PermissionEndpoint struct {
}

func NewPermissionEndpoint() (*PermissionEndpoint, error) {
	return &PermissionEndpoint{}, nil
}

func (e *PermissionEndpoint) PermissionCreate(ctx Context, permission Permission) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}

func (e *PermissionEndpoint) PermissionGet(ctx Context, id PermissionId) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}

func (e *PermissionEndpoint) PermissionGetByName(ctx Context, name string, organizationId OrganizationId) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}

func (e *PermissionEndpoint) PermissionGetAll(ctx Context, organizationId OrganizationId) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}
