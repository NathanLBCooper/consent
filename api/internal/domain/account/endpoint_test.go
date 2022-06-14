package account_test

import (
	"testing"

	"github.com/stretchr/testify/assert"
	"nathan.dev/consent/internal/domain/account"
	"nathan.dev/consent/internal/testcommon"
)

type EndpointTest struct {
	ctx *testcommon.FakeContext
	sut *account.Endpoint
}

func setupEndpointTest(t *testing.T) *EndpointTest {
	fakeRepo := &testcommon.FakeAccountRepo{}

	endpoint, err := account.NewEndpoint(fakeRepo)
	assert.NoError(t, err)

	return &EndpointTest{
		ctx: testcommon.NewFakeContext("someid"),
		sut: endpoint,
	}
}

func Test_create_and_get_user(t *testing.T) {
	state := setupEndpointTest(t)

	request := account.UserCreateRequest{Name: "testuser"}
	model, err := state.sut.CreateUser(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.GetUser(state.ctx, account.UserGetRequest{Id: model.Id})
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_and_get_organization(t *testing.T) {
	state := setupEndpointTest(t)

	user, err := state.sut.CreateUser(state.ctx, account.UserCreateRequest{Name: "user"})
	assert.NoError(t, err)

	request := account.OrganizationCreateRequest{Name: "testorganization", OwnerUserId: user.Id}
	model, err := state.sut.CreateOrganization(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.GetOrganization(state.ctx, account.OrganizationGetRequest{Id: model.Id})
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_organization_throws_if_owning_user_doesnt_exist(t *testing.T) {
	state := setupEndpointTest(t)

	request := account.OrganizationCreateRequest{Name: "testorganization", OwnerUserId: "thisdoesntexist"}
	_, err := state.sut.CreateOrganization(state.ctx, request)

	assert.NotNil(t, err)
	assert.Equal(t, "Owning user: not found", err.Error())
}
