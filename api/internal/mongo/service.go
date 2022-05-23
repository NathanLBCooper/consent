package mongo

import (
	"context"
	"time"

	"github.com/google/uuid"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"nathan.dev/consent/internal"
)

type Service struct {
	conn *mongo.Client
	db   *mongo.Database
}

var _ internal.Database = &Service{}

func New(uri, database string) (*Service, error) {
	conn, err := mongo.Connect(context.Background(), options.Client().ApplyURI(uri))
	if err != nil {
		return nil, err
	}
	if err := conn.Ping(context.Background(), nil); err != nil {
		return nil, err
	}
	db := conn.Database(database)
	return &Service{conn: conn, db: db}, nil
}

func (s *Service) AccountCreate(ctx context.Context, v internal.Account) (*internal.AccountModel, error) {
	now := time.Now()
	doc := &internal.AccountModel{
		Model:   internal.Model{ID: uuid.NewString(), Created: now, Updated: now},
		Account: v,
	}
	if _, err := s.db.Collection("account").InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (s *Service) AccountGet(ctx context.Context, id string) (*internal.AccountModel, error) {
	var doc internal.AccountModel
	if err := s.db.Collection("account").FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}

func (s *Service) OrganizationCreate(ctx context.Context, v internal.Organization) (*internal.OrganizationModel, error) {
	now := time.Now()
	doc := &internal.OrganizationModel{
		Model:        internal.Model{ID: uuid.NewString(), Created: now, Updated: now},
		Organization: v,
	}
	if _, err := s.db.Collection("organization").InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (s *Service) OrganizationGet(ctx context.Context, id string) (*internal.OrganizationModel, error) {
	var doc internal.OrganizationModel
	if err := s.db.Collection("organization").FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}
