package context

import (
	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
)

type Context struct {
	*gin.Context
	correlationId string
}

const headerName string = "session"

func NewContext(ctx *gin.Context) *Context {
	return &Context{Context: ctx, correlationId: ctx.GetString(headerName)}
}

func (context *Context) CorrelationId() string { return context.correlationId }

func EnrichHeader(ctx *gin.Context) {
	header := ctx.GetHeader(headerName)
	if header == "" {
		header = uuid.NewString()
		ctx.Header(headerName, header)
	}
	ctx.Set(headerName, header)
	ctx.Next()
}
