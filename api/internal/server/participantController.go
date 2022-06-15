package server

import (
	"github.com/gin-gonic/gin"

	"nathan.dev/consent/internal/domain"
)

type participantController struct {
	endpoint *domain.ParticipantEndpoint
	session  sessionStorage
	engine   *gin.Engine
}

func newParticipantController(endpoint *domain.ParticipantEndpoint, session sessionStorage, engine *gin.Engine) (*participantController, error) {
	return &participantController{endpoint: endpoint, session: session, engine: engine}, nil
}

// @Summary      Get an participant
// @Param        id   path      int  true  "User Id"
// @Success      200  {object}  domain.ParticipantModel
// @Router       /v1/participant/{id} [get]
func (c *participantController) ParticipantGet(ctx *gin.Context) {
	model, err := c.endpoint.ParticipantGet(newServerContext(ctx), domain.ParticipantGetRequest{Id: ctx.Param("id")})
	if err != nil {
		ctx.JSON(404, gin.H{"status": "not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// todo
