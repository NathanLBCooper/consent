package domain

import "errors"

// A point in the Participant journey
// Eg: Registration, Payment etc
type Touchpoint struct {
	Name string
}

type TouchpointModel struct {
	Model
	Touchpoint
}

type TouchpointEndpoint struct {
}

func NewTouchpointEndpoint() (*TouchpointEndpoint, error) {
	return &TouchpointEndpoint{}, nil
}

func (e *TouchpointEndpoint) TouchpointCreate(ctx Context, permission Permission) (*TouchpointModel, error) {
	return nil, errors.New("not implemented")
}

func (e *TouchpointEndpoint) TouchpointGet(ctx Context, id string) (*TouchpointModel, error) {
	return nil, errors.New("not implemented")
}

func (e *TouchpointEndpoint) TouchpointGetByName(ctx Context, name string, organizationId string) (*TouchpointModel, error) {
	return nil, errors.New("not implemented")
}

func (e *TouchpointEndpoint) TouchpointGetAll(ctx Context, organizationId string) (*TouchpointModel, error) {
	return nil, errors.New("not implemented")
}
