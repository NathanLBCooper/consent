package domain_test

import (
	"testing"

	"github.com/stretchr/testify/assert"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/testcommon"
)

type endpointTest struct {
	ctx *testcommon.FakeContext
	sut *domain.AccountEndpoint
}

func setupEndpointTest(t *testing.T) *endpointTest {
	fakeRepo := &testcommon.FakeAccountRepo{}

	endpoint, err := domain.NewAccountEndpoint(fakeRepo)
	assert.NoError(t, err)

	return &endpointTest{
		ctx: testcommon.NewFakeContext("someid"),
		sut: endpoint,
	}
}

func Test_create_and_get_user(t *testing.T) {
	state := setupEndpointTest(t)

	request := domain.UserCreateRequest{Name: "testuser"}
	model, err := state.sut.UserCreate(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.UserGet(state.ctx, domain.UserGetRequest{Id: model.Id})
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_and_get_organization(t *testing.T) {
	state := setupEndpointTest(t)

	user, err := state.sut.UserCreate(state.ctx, domain.UserCreateRequest{Name: "user"})
	assert.NoError(t, err)

	request := domain.OrganizationCreateRequest{Name: "testorganization", OwnerUserId: user.Id}
	model, err := state.sut.OrganizationCreate(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.OrganizationGet(state.ctx, domain.OrganizationGetRequest{Id: model.Id})
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_organization_throws_if_owning_user_doesnt_exist(t *testing.T) {
	state := setupEndpointTest(t)

	request := domain.OrganizationCreateRequest{Name: "testorganization", OwnerUserId: "thisdoesntexist"}
	_, err := state.sut.OrganizationCreate(state.ctx, request)

	assert.NotNil(t, err)
	assert.Equal(t, "Owning user: not found", err.Error())
}
