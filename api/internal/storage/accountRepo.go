package storage

import (
	"context"
	"time"

	"github.com/google/uuid"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/domain/account"
)

type AccountRepo struct {
	db *mongo.Database
}

var _ account.Repo = &AccountRepo{}

func NewAccountRepo(db *mongo.Database) (*AccountRepo, error) {
	return &AccountRepo{db: db}, nil
}

const userKey string = "user"
const organizationKey string = "organization"

func (s *AccountRepo) CreateUser(ctx context.Context, v account.User) (*account.UserModel, error) {
	now := time.Now()
	doc := &account.UserModel{
		Model: domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		User:  v,
	}
	if _, err := s.db.Collection(userKey).InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (s *AccountRepo) GetUser(ctx context.Context, id string) (*account.UserModel, error) {
	var doc account.UserModel
	if err := s.db.Collection(userKey).FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}

func (s *AccountRepo) CreateOrganization(ctx context.Context, v account.Organization) (*account.OrganizationModel, error) {
	now := time.Now()
	doc := &account.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: v,
	}
	if _, err := s.db.Collection(organizationKey).InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (s *AccountRepo) GetOrganization(ctx context.Context, id string) (*account.OrganizationModel, error) {
	var doc account.OrganizationModel
	if err := s.db.Collection(organizationKey).FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}
