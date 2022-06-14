package application

import "nathan.dev/consent/internal/domain"

// Some touchpoint eg Registration
type Touchpoint struct {
	Name string
}

type TouchpointModel struct {
	domain.Model
	Touchpoint
}
