package storage

import (
	"context"
	"time"

	"github.com/google/uuid"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"nathan.dev/consent/internal/domain"
)

type AccountRepo struct {
	db *mongo.Database
}

var _ domain.AccountRepo = &AccountRepo{}

func NewAccountRepo(db *mongo.Database) (*AccountRepo, error) {
	return &AccountRepo{db: db}, nil
}

const userKey string = "user"
const organizationKey string = "organization"

func (r *AccountRepo) UserCreate(ctx context.Context, user domain.User) (*domain.UserModel, error) {
	now := time.Now()
	doc := &domain.UserModel{
		Model: domain.Model[domain.UserId]{Id: domain.UserId(uuid.New()), Created: now, Updated: now},
		User:  user,
	}
	if _, err := r.db.Collection(userKey).InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (r *AccountRepo) UserGet(ctx context.Context, id domain.UserId) (*domain.UserModel, error) {
	var doc domain.UserModel
	if err := r.db.Collection(userKey).FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}

func (r *AccountRepo) OrganizationCreate(ctx context.Context, org domain.Organization) (*domain.OrganizationModel, error) {
	now := time.Now()
	doc := &domain.OrganizationModel{
		Model:        domain.Model[domain.OrganizationId]{Id: domain.OrganizationId(uuid.New()), Created: now, Updated: now},
		Organization: org,
	}
	if _, err := r.db.Collection(organizationKey).InsertOne(ctx, doc); err != nil {
		return nil, err
	}
	return doc, nil
}

func (r *AccountRepo) OrganizationGet(ctx context.Context, id domain.OrganizationId) (*domain.OrganizationModel, error) {
	var doc domain.OrganizationModel
	if err := r.db.Collection(organizationKey).FindOne(ctx, bson.M{"model.id": id}).Decode(&doc); err != nil {
		return nil, err
	}
	return &doc, nil
}
