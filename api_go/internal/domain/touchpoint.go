package domain

import (
	"github.com/google/uuid"
)

// A point in the Participant journey
// Eg: Registration, Payment etc
type Touchpoint struct {
	Name string
}

type TouchpointId uuid.UUID
type TouchpointModel struct {
	Model[TouchpointId]
	Touchpoint
}
