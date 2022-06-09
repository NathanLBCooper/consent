package user_test

import (
	"testing"

	"github.com/stretchr/testify/assert"
	"nathan.dev/consent/internal/domain/user"
	"nathan.dev/consent/internal/testcommon"
)

type EndpointTest struct {
	ctx *testcommon.FakeContext
	sut *user.Endpoint
}

func setupEndpointTest(t *testing.T) *EndpointTest {
	fakeRepo := &testcommon.FakeUserRepo{}

	endpoint, err := user.NewEndpoint(fakeRepo)
	assert.NoError(t, err)

	return &EndpointTest{
		ctx: testcommon.NewFakeContext("someid"),
		sut: endpoint,
	}
}

func Test_create_and_get_account(t *testing.T) {
	state := setupEndpointTest(t)

	request := user.AccountCreateRequest{Name: "testaccount"}
	model, err := state.sut.CreateAccount(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.GetAccount(state.ctx, user.AccountGetRequest{Id: model.Id})
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_and_get_organization(t *testing.T) {
	state := setupEndpointTest(t)

	account, err := state.sut.CreateAccount(state.ctx, user.AccountCreateRequest{Name: "account"})
	assert.NoError(t, err)

	request := user.OrganizationCreateRequest{Name: "testorganization", OwnerAccountId: account.Id}
	model, err := state.sut.CreateOrganization(state.ctx, request)
	assert.NoError(t, err)
	assert.Equal(t, request.Name, model.Name)

	result, err := state.sut.GetOrganization(state.ctx, user.OrganizationGetRequest{Id: model.Id})
	assert.NoError(t, err)

	assert.Equal(t, request.Name, result.Name)
}

func Test_create_organization_throws_if_owning_account_doesnt_exist(t *testing.T) {
	state := setupEndpointTest(t)

	request := user.OrganizationCreateRequest{Name: "testorganization", OwnerAccountId: "thisdoesntexist"}
	_, err := state.sut.CreateOrganization(state.ctx, request)

	assert.NotNil(t, err)
	assert.Equal(t, "Owning account: not found", err.Error())
}
