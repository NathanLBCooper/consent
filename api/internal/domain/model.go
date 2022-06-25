package domain

import (
	"time"

	"github.com/google/uuid"
)

type Model struct {
	Id      uuid.UUID
	Created time.Time
	Updated time.Time
	Removed bool
}
