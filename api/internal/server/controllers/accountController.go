package controllers

import (
	"github.com/gin-gonic/gin"
	"nathan.dev/consent/internal/domain/account"
	"nathan.dev/consent/internal/server/context"
)

type AccountController struct {
	endpoint *account.AccountEndpoint
	session  context.SessionStorage
	engine   *gin.Engine
}

func NewAccountController(endpoint *account.AccountEndpoint, session context.SessionStorage, engine *gin.Engine) (*AccountController, error) {
	return &AccountController{endpoint: endpoint, session: session, engine: engine}, nil
}

// @Summary      Get an user
// @Param        id   path      int  true  "User Id"
// @Success      200  {object}  account.UserModel
// @Router       /v1/account/user/{id} [get]
func (c *AccountController) UserGet(ctx *gin.Context) {
	model, err := c.endpoint.UserGet(context.NewContext(ctx), account.UserGetRequest{Id: ctx.Param("id")})
	if err != nil {
		ctx.JSON(404, gin.H{"status": "not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Create a user
// @Param        user     body      account.UserCreateRequest  true  "Create user"
// @Success      200      {object}  account.UserModel
// @Router       /v1/account/user [post]
func (c *AccountController) UserCreate(ctx *gin.Context) {
	var request account.UserCreateRequest
	if err := ctx.BindJSON(&request); err != nil {
		ctx.JSON(500, gin.H{"status": "todo 1"})
		return
	}

	model, err := c.endpoint.UserCreate(context.NewContext(ctx), request)
	if err != nil {
		ctx.JSON(500, gin.H{"status": "todo 2"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Get an Organization
// @Param        id   path      int  true  "Organization Id"
// @Success      200  {object}  account.OrganizationModel
// @Router       /v1/account/organization/{id} [get]
func (c *AccountController) OrganizationGet(ctx *gin.Context) {
	ctx.JSON(500, gin.H{"status": "not implemented"})
}

type OrganizationCreateRequest struct {
	Name string
}

// @Summary      Create an Organization
// @Param        organization     body      OrganizationCreateRequest  true  "Create organization"
// @Success      200  {object}  account.OrganizationModel
// @Router       /v1/account/organization [post]
func (c *AccountController) OrganizationCreate(ctx *gin.Context) {
	context := context.NewContext(ctx)
	id := c.session.Get(context.CorrelationId(), "user")
	if id == "" {
		ctx.JSON(401, gin.H{"status": "not logged in"})
		return
	}
	ctx.JSON(500, gin.H{"status": "not implemented"})
}
