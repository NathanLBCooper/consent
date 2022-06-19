package domain

import "errors"

// A specific idea that can be agreed to
// Eg: 'Receive information and offers', 'Use of cookies', 'Share your knäckebröd preferences with Wasa"
type Permission struct {
	Name           string
	OrganizationId string
}

type PermissionModel struct {
	Model
	Permission
}

type PermissionEndpoint struct {
}

func NewPermissionEndpoint() (*PermissionEndpoint, error) {
	return &PermissionEndpoint{}, nil
}

func (e *PermissionEndpoint) PermissionCreate(ctx Context, permission Permission) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}

func (e *PermissionEndpoint) PermissionGet(ctx Context, id string) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}

func (e *PermissionEndpoint) PermissionGetByName(ctx Context, name string, organizationId string) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}

func (e *PermissionEndpoint) PermissionGetAll(ctx Context, organizationId string) (*PermissionModel, error) {
	return nil, errors.New("not implemented")
}
