package storage

import (
	"context"

	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type MongoConnection struct {
	Client *mongo.Client
	Db     *mongo.Database
}

type MongoConnectionSettings struct {
	Uri    string
	DbName string
}

func NewMongoConnection(settings MongoConnectionSettings) (*MongoConnection, error) {
	client, err := mongo.Connect(context.Background(), options.Client().ApplyURI(settings.Uri))
	if err != nil {
		return nil, err
	}
	if err := client.Ping(context.Background(), nil); err != nil {
		return nil, err
	}
	db := client.Database(settings.DbName)
	return &MongoConnection{Client: client, Db: db}, nil
}
