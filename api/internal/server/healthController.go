package server

import (
	"context"
	"net/http"

	"github.com/gin-gonic/gin"
	"nathan.dev/consent/internal/storage"
)

type healthController struct {
	engine          *gin.Engine
	mongoConnection *storage.MongoConnection
}

func newHealthController(mongoConnection *storage.MongoConnection, engine *gin.Engine) (*healthController, error) {
	return &healthController{mongoConnection: mongoConnection, engine: engine}, nil
}

type healthResponse struct {
	Components []componentHealth
}

type componentHealth struct {
	Name      string
	IsHealthy bool
	Detail    string
}

// Health godoc
// @Summary      Get Health
// @Description  Get component by component health status
// @Success      200  {object}  healthResponse
// @Router       /v1/health [get]
func (c *healthController) Get(ctx *gin.Context) {
	response := healthResponse{
		Components: []componentHealth{
			{Name: "api", IsHealthy: true},
			c.checkMongo(),
		},
	}

	ctx.JSON(http.StatusOK, response)
}

func (c *healthController) checkMongo() componentHealth {
	const name string = "mongo"
	if err := c.mongoConnection.Client.Ping(context.Background(), nil); err != nil {
		return componentHealth{Name: name, IsHealthy: false, Detail: err.Error()}
	}

	return componentHealth{Name: name, IsHealthy: true, Detail: ""}
}
