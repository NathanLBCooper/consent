package domain

import "time"

type Model struct {
	Id      string
	Created time.Time
	Updated time.Time
	Removed bool
}
