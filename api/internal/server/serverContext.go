package server

import (
	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

type serverContext struct {
	*gin.Context
	correlationId string
}

const headerName string = "session"

func newServerContext(ctx *gin.Context) *serverContext {
	return &serverContext{Context: ctx, correlationId: ctx.GetString(headerName)}
}

func (context *serverContext) CorrelationId() string { return context.correlationId }

func enrichHeader(ctx *gin.Context) {
	header := ctx.GetHeader(headerName)
	if header == "" {
		header = uuid.NewString()
		ctx.Header(headerName, header)
	}
	ctx.Set(headerName, header)
	ctx.Next()
}
