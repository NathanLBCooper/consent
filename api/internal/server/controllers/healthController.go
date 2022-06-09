package controllers

import (
	"context"
	"net/http"

	"github.com/gin-gonic/gin"
	"nathan.dev/consent/internal/storage"
)

type HealthController struct {
	engine          *gin.Engine
	mongoConnection *storage.MongoConnection
}

func NewHealthController(mongoConnection *storage.MongoConnection, engine *gin.Engine) (*HealthController, error) {
	return &HealthController{mongoConnection: mongoConnection, engine: engine}, nil
}

type HealthResponse struct {
	Components []ComponentHealth
}

type ComponentHealth struct {
	Name      string
	IsHealthy bool
	Detail    string
}

// Health godoc
// @Summary      Get Health
// @Description  Get component by component health status
// @Success      200  {object}  HealthResponse
// @Router       /v1/health [get]
func (controller *HealthController) Health(c *gin.Context) {
	response := HealthResponse{
		Components: []ComponentHealth{
			{Name: "api", IsHealthy: true},
			controller.checkMongo(),
		},
	}

	c.JSON(http.StatusOK, response)
}

func (controller *HealthController) checkMongo() ComponentHealth {
	const name string = "mongo"
	if err := controller.mongoConnection.Client.Ping(context.Background(), nil); err != nil {
		return ComponentHealth{Name: name, IsHealthy: false, Detail: err.Error()}
	}

	return ComponentHealth{Name: name, IsHealthy: true, Detail: ""}
}
