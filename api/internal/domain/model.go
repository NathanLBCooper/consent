package domain

import (
	"time"
)

type Model[TIdentity ~[16]byte] struct {
	// [16]byte is underlying type of uuid.UUID
	Id      TIdentity
	Created time.Time
	Updated time.Time
	Removed bool
}
