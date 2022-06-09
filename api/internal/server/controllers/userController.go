package controllers

import (
	"github.com/gin-gonic/gin"
	"nathan.dev/consent/internal/domain/user"
	"nathan.dev/consent/internal/server/context"
)

type UserController struct {
	endpoint *user.Endpoint
	session  context.SessionStorage
	engine   *gin.Engine
}

func NewUsersController(endpoint *user.Endpoint, session context.SessionStorage, engine *gin.Engine) (*UserController, error) {
	return &UserController{endpoint: endpoint, session: session, engine: engine}, nil
}

// @Summary      Get an account
// @Param        id   path      int  true  "Account Id"
// @Success      200  {object}  user.AccountModel
// @Router       /v1/account/{id} [get]
func (controller *UserController) GetAccount(ctx *gin.Context) {
	v, err := controller.endpoint.GetAccount(context.NewContext(ctx), user.AccountGetRequest{Id: ctx.Param("id")})
	if err != nil {
		ctx.JSON(404, gin.H{"status": "not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": v})
}

// @Summary      Get an Organization
// @Param        id   path      int  true  "Organization Id"
// @Success      200  {object}  OrganizationCreateRequest
// @Router       /v1/organization/{id} [get]
func (controller *UserController) GetOrganization(ctx *gin.Context) {
	ctx.JSON(500, gin.H{"status": "not implemented"})
}

type OrganizationCreateRequest struct {
	Name string
}

// todo docs and hook up
func (controller *UserController) CreateOrganization(ctx *gin.Context) {
	context := context.NewContext(ctx)
	id := controller.session.Get(context.CorrelationId(), "account")
	if id == "" {
		ctx.JSON(401, gin.H{"status": "not logged in"})
		return
	}
	ctx.JSON(500, gin.H{"status": "not implemented"})
}
