package httpserver

import (
	"log"

	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
	"nathan.dev/consent/internal"
)

type Service struct {
	api    *internal.API
	engine *gin.Engine
}

func New(api *internal.API, host string) (*Service, error) {
	s := &Service{api: api, engine: gin.Default()}
	go s.engine.Run(host)
	s.engine.Use(func(ctx *gin.Context) {
		session := ctx.GetHeader("session")
		if session == "" {
			session = uuid.NewString()
			ctx.Header("session", session)
		}
		ctx.Set("session", session)
		log.Println("session", session)
		ctx.Next()
	})
	s.engine.GET("/account/:id", func(ctx *gin.Context) {
		v, err := api.AccountGet(&Context{Context: ctx, id: ctx.GetString("session")}, internal.AccountGetRequest{ID: ctx.Param("id")})
		if err != nil {
			ctx.JSON(404, gin.H{"status": "not found"})
			return
		}
		ctx.JSON(200, gin.H{"status": v})
	})
	return s, nil
}

type Context struct {
	*gin.Context
	id string
}

func (c *Context) ID() string { return c.id }

func (s *Service) Close() {}
