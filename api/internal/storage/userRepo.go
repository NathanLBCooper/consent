package storage

import (
	"context"
	"time"

	"github.com/google/uuid"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/domain/user"
)

type UserRepo struct {
	db *mongo.Database
}

var _ user.Repo = &UserRepo{}

func NewUserRepo(db *mongo.Database) (*UserRepo, error) {
	return &UserRepo{db: db}, nil
}

func (s *UserRepo) CreateAccount(ctx context.Context, v user.Account) (*user.AccountModel, error) {
	now := time.Now()
	doc := &user.AccountModel{
		Model:   domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Account: v,
	}
	if _, err := s.db.Collection("account").InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (s *UserRepo) GetAccount(ctx context.Context, id string) (*user.AccountModel, error) {
	var doc user.AccountModel
	if err := s.db.Collection("account").FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}

func (s *UserRepo) CreateOrganization(ctx context.Context, v user.Organization) (*user.OrganizationModel, error) {
	now := time.Now()
	doc := &user.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: v,
	}
	if _, err := s.db.Collection("organization").InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (s *UserRepo) GetOrganization(ctx context.Context, id string) (*user.OrganizationModel, error) {
	var doc user.OrganizationModel
	if err := s.db.Collection("organization").FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}
