package domain_test

import (
	"testing"

	"github.com/google/uuid"
	"github.com/stretchr/testify/assert"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/testcommon"
)

type accountTest struct {
	ctx *testcommon.FakeContext
	sut *domain.AccountEndpoint
}

func setupAccountTest(t *testing.T) *accountTest {
	fakeRepo := &testcommon.FakeAccountRepo{}

	endpoint, err := domain.NewAccountEndpoint(fakeRepo)
	assert.NoError(t, err)

	return &accountTest{
		ctx: testcommon.NewFakeContext("someid"),
		sut: endpoint,
	}
}

func Test_create_and_get_user(t *testing.T) {
	state := setupAccountTest(t)

	request := domain.User{Name: "testuser"}
	model, err := state.sut.UserCreate(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.UserGet(state.ctx, model.Id)
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_and_get_organization(t *testing.T) {
	state := setupAccountTest(t)

	user, err := state.sut.UserCreate(state.ctx, domain.User{Name: "user"})
	assert.NoError(t, err)

	request := domain.OrganizationCreateRequest{Name: "testorganization", OwnerUserId: user.Id}
	model, err := state.sut.OrganizationCreate(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.OrganizationGet(state.ctx, model.Id)
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_organization_throws_if_owning_user_doesnt_exist(t *testing.T) {
	state := setupAccountTest(t)

	request := domain.OrganizationCreateRequest{Name: "testorganization", OwnerUserId: uuid.New()}
	_, err := state.sut.OrganizationCreate(state.ctx, request)

	assert.NotNil(t, err)
	assert.Equal(t, "Owning user: not found", err.Error())
}
