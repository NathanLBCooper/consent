package domain

import "context"

type Context interface {
	context.Context
	CorrelationId() string
}
