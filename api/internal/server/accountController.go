package server

import (
	"github.com/gin-gonic/gin"
	"nathan.dev/consent/internal/domain"
)

type accountController struct {
	endpoint *domain.AccountEndpoint
	session  sessionStorage
	engine   *gin.Engine
}

func newAccountController(endpoint *domain.AccountEndpoint, session sessionStorage, engine *gin.Engine) (*accountController, error) {
	return &accountController{endpoint: endpoint, session: session, engine: engine}, nil
}

// @Summary      Get an user
// @Param        id   path      int  true  "User Id"
// @Success      200  {object}  domain.UserModel
// @Router       /v1/account/user/{id} [get]
func (c *accountController) userGet(ctx *gin.Context) {
	model, err := c.endpoint.UserGet(newServerContext(ctx), domain.UserGetRequest{Id: ctx.Param("id")})
	if err != nil {
		ctx.JSON(404, gin.H{"status": "not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Create a user
// @Param        user     body      domain.UserCreateRequest  true  "Create user"
// @Success      200      {object}  domain.UserModel
// @Router       /v1/account/user [post]
func (c *accountController) userCreate(ctx *gin.Context) {
	var request domain.UserCreateRequest
	if err := ctx.BindJSON(&request); err != nil {
		ctx.JSON(500, gin.H{"status": "todo 1"})
		return
	}

	model, err := c.endpoint.UserCreate(newServerContext(ctx), request)
	if err != nil {
		ctx.JSON(500, gin.H{"status": "todo 2"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Get an Organization
// @Param        id   path      int  true  "Organization Id"
// @Success      200  {object}  domain.OrganizationModel
// @Router       /v1/account/organization/{id} [get]
func (c *accountController) organizationGet(ctx *gin.Context) {
	ctx.JSON(500, gin.H{"status": "not implemented"})
}

type organizationCreateRequest struct {
	Name string
}

// @Summary      Create an Organization
// @Param        organization     body      organizationCreateRequest  true  "Create organization"
// @Success      200  {object}  domain.OrganizationModel
// @Router       /v1/account/organization [post]
func (c *accountController) organizationCreate(ctx *gin.Context) {
	context := newServerContext(ctx)
	id := c.session.Get(context.CorrelationId(), "user")
	if id == "" {
		ctx.JSON(401, gin.H{"status": "not logged in"})
		return
	}

	var request organizationCreateRequest
	if err := ctx.BindJSON(&request); err != nil {
		ctx.JSON(500, gin.H{"status": "todo 1"})
		return
	}

	ctx.JSON(500, gin.H{"status": "not implemented"})
}
