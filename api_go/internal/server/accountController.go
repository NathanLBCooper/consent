package server

import (
	"github.com/gin-gonic/gin"
	"github.com/google/uuid"
	"nathan.dev/consent/internal/domain"
)

type organizationCreateRequest struct {
	Name string
}

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
	id, err := uuid.Parse(ctx.Param("id"))
	if err != nil {
		ctx.JSON(400, gin.H{"status": "id parameter is not valid"})
		return
	}

	model, err := c.endpoint.UserGet(newServerContext(ctx), domain.UserId(id))
	if err != nil {
		ctx.JSON(404, gin.H{"status": "user not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Create a user
// @Param        user     body      domain.User  true  "Create user"
// @Success      200      {object}  domain.UserModel
// @Router       /v1/account/user [post]
func (c *accountController) userCreate(ctx *gin.Context) {
	var request domain.User
	if err := ctx.BindJSON(&request); err != nil {
		ctx.JSON(400, gin.H{"status": "request body is not valid"})
		return
	}

	model, err := c.endpoint.UserCreate(newServerContext(ctx), request)
	if err != nil {
		ctx.JSON(500, gin.H{"status": "error creating user"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Get an Organization
// @Param        id   path      int  true  "Organization Id"
// @Success      200  {object}  domain.OrganizationModel
// @Router       /v1/account/organization/{id} [get]
func (c *accountController) organizationGet(ctx *gin.Context) {
	id, err := uuid.Parse(ctx.Param("id"))
	if err != nil {
		ctx.JSON(400, gin.H{"status": "id parameter is not valid"})
		return
	}

	model, err := c.endpoint.OrganizationGet(newServerContext(ctx), domain.OrganizationId(id))
	if err != nil {
		ctx.JSON(404, gin.H{"status": "organization not found"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}

// @Summary      Create an Organization
// @Param        organization     body      organizationCreateRequest  true  "Create organization"
// @Success      200  {object}  domain.OrganizationModel
// @Router       /v1/account/organization [post]
func (c *accountController) organizationCreate(ctx *gin.Context) {
	//context := newServerContext(ctx)
	// userId := c.session.Get(context.CorrelationId(), "user")
	// if userId == "" {
	// 	ctx.JSON(401, gin.H{"status": "requesting user is not logged in"})
	// 	return
	// }

	// todo get userid
	userId := uuid.New()

	var request organizationCreateRequest
	if err := ctx.BindJSON(&request); err != nil {
		ctx.JSON(500, gin.H{"status": "request body is not valid"})
		return
	}

	createRequest := domain.OrganizationCreateRequest{Name: request.Name, OwnerUserId: domain.UserId(userId)}

	model, err := c.endpoint.OrganizationCreate(newServerContext(ctx), createRequest)
	if err != nil {
		ctx.JSON(500, gin.H{"status": "error creating organization"})
		return
	}

	ctx.JSON(200, gin.H{"status": model})
}
