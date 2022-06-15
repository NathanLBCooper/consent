package controllers

import (
	"github.com/gin-gonic/gin"

	"nathan.dev/consent/internal/domain/participant"
	"nathan.dev/consent/internal/server/context"
)

type ParticipantController struct {
	endpoint *participant.ParticipantEndpoint
	session  context.SessionStorage
	engine   *gin.Engine
}

func NewParticipantController(endpoint *participant.ParticipantEndpoint, session context.SessionStorage, engine *gin.Engine) (*ParticipantController, error) {
	return &ParticipantController{endpoint: endpoint, session: session, engine: engine}, nil
}

// @Summary      Get an participant
// @Param        id   path      int  true  "User Id"
// @Success      200  {object}  participant.ParticipantModel
// @Router       /v1/participant/{id} [get]
func (c *ParticipantController) ParticipantGet(ctx *gin.Context) {
	model, err := c.endpoint.ParticipantGet(context.NewContext(ctx), participant.ParticipantGetRequest{Id: ctx.Param("id")})
	if err != nil {
		ctx.JSON(404, gin.H{"status": "not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// todo
