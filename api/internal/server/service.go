package server

import (
	"log"

	"github.com/gin-gonic/gin"
	swaggerFiles "github.com/swaggo/files"
	ginSwagger "github.com/swaggo/gin-swagger"

	"nathan.dev/consent/internal/domain/user"
	"nathan.dev/consent/internal/server/context"
	"nathan.dev/consent/internal/server/controllers"
	"nathan.dev/consent/internal/storage"
)

type Service struct {
	engine    *gin.Engine
	container *container
}

type Config struct {
	MongoConnectionSettings storage.MongoConnectionSettings
}

// @title           Consent API
// @version         1.0
func StartService(config *Config) (*Service, error) {
	s := &Service{engine: gin.Default()}
	s.engine.Use(context.EnrichHeader)
	s.container = configureDependencies(s.engine, config)
	s.configureRoutes()

	go s.engine.Run(":3001")

	return s, nil
}

func (s *Service) Close() {}

func (s *Service) configureRoutes() {
	s.engine.GET("/swagger/*any", ginSwagger.WrapHandler(swaggerFiles.Handler))

	v1 := s.engine.Group("/v1")
	{
		v1.GET("/health", s.container.healthController.Health)

		account := v1.Group("/account")
		{
			account.GET(":id", s.container.userController.GetAccount)
			account.POST("", s.container.userController.CreateAccount)
		}

		Organization := v1.Group("/organization")
		{
			Organization.GET(":id", s.container.userController.GetOrganization)
		}
	}
}

type container struct {
	healthController *controllers.HealthController
	userController   *controllers.UserController
}

func configureDependencies(engine *gin.Engine, config *Config) *container {
	mongoConnection, err := storage.NewMongoConnection(config.MongoConnectionSettings)
	if err != nil {
		log.Fatalln("mongo init fail", err)
	}

	userRepo, err := storage.NewUserRepo(mongoConnection.Db)
	if err != nil {
		log.Fatalln("userRepo init fail", err)
	}

	userEndpoint, err := user.NewEndpoint(userRepo)
	if err != nil {
		log.Fatalln("userEndpoint init fail", err)
	}

	healthController, err := controllers.NewHealthController(mongoConnection, engine)
	if err != nil {
		log.Fatalln("healthController init fail", err)
	}

	userController, err := controllers.NewUsersController(userEndpoint, nil, engine)
	if err != nil {
		log.Fatalln("accountController init fail", err)
	}

	return &container{healthController: healthController, userController: userController}
}
