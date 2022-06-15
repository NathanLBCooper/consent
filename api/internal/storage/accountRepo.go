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

var _ account.AccountRepo = &AccountRepo{}

func NewAccountRepo(db *mongo.Database) (*AccountRepo, error) {
	return &AccountRepo{db: db}, nil
}

const userKey string = "user"
const organizationKey string = "organization"

func (r *AccountRepo) UserCreate(ctx context.Context, user account.User) (*account.UserModel, error) {
	now := time.Now()
	doc := &account.UserModel{
		Model: domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		User:  user,
	}
	if _, err := r.db.Collection(userKey).InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (r *AccountRepo) UserGet(ctx context.Context, id string) (*account.UserModel, error) {
	var doc account.UserModel
	if err := r.db.Collection(userKey).FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}

func (r *AccountRepo) OrganizationCreate(ctx context.Context, org account.Organization) (*account.OrganizationModel, error) {
	now := time.Now()
	doc := &account.OrganizationModel{
		Model:        domain.Model{Id: uuid.NewString(), Created: now, Updated: now},
		Organization: org,
	}
	if _, err := r.db.Collection(organizationKey).InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (r *AccountRepo) OrganizationGet(ctx context.Context, id string) (*account.OrganizationModel, error) {
	var doc account.OrganizationModel
	if err := r.db.Collection(organizationKey).FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}
