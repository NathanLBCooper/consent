package testcommon

import "context"

type FakeContext struct {
	context.Context
	correlationId string
}

func NewFakeContext(correlationId string) *FakeContext {
	return &FakeContext{correlationId: correlationId}
}

func (context *FakeContext) CorrelationId() string { return context.correlationId }
