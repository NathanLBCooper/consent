package server

import (
	"log"

	"github.com/gin-gonic/gin"
	"nathan.dev/consent/internal/domain/user"
	"nathan.dev/consent/internal/server/controllers"
	"nathan.dev/consent/internal/storage"
)

type container struct {
	healthController *controllers.HealthController
	userController   *controllers.UserController
}

func configureDependencies(engine *gin.Engine) *container {

	// todo this is configuration
	//mongoConnectionSettings := storage.MongoConnectionSettings{Uri: "mongodb://mongo:27017", DbName: "consent"} // docker->docker
	mongoConnectionSettings := storage.MongoConnectionSettings{Uri: "mongodb://localhost:33002", DbName: "consent"} // local->docker

	mongoConnection, err := storage.NewMongoConnection(mongoConnectionSettings)
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
