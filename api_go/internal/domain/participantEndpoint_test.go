package domain_test

import (
	"testing"

	"github.com/google/uuid"
	"github.com/stretchr/testify/assert"
	"nathan.dev/consent/internal/domain"
	"nathan.dev/consent/internal/testcommon"
)

type participantTest struct {
	ctx *testcommon.FakeContext
	sut *domain.ParticipantEndpoint
}

func setupParticipantTest(t *testing.T) *participantTest {
	fakeRepo := &testcommon.FakeParticipantRepo{}

	endpoint, err := domain.NewParticipantEndpoint(fakeRepo)
	assert.NoError(t, err)

	return &participantTest{
		ctx: testcommon.NewFakeContext("someid"),
		sut: endpoint,
	}
}

func Test_create_and_get_participant(t *testing.T) {
	state := setupParticipantTest(t)

	request := domain.Participant{
		ExternalId: "testParticipant", OrganizationId: domain.OrganizationId(uuid.New()),
		Tags: []domain.Tag{{Key: "tag1"}, {Key: "tag2"}}}

	model, err := state.sut.ParticipantCreate(state.ctx, request)
	assert.NoError(t, err)
	assertParticipantsEqual(t, &request, &model.Participant)
	assert.Equal(t, request.OrganizationId, model.OrganizationId)
	assert.Equal(t, len(request.Tags), len(model.Tags))

	result, err := state.sut.ParticipantGet(state.ctx, model.Id)
	assert.NoError(t, err)

	assertParticipantsEqual(t, &request, &result.Participant)
}

func Test_get_participant_by_external_id(t *testing.T) {
	state := setupParticipantTest(t)

	request := domain.Participant{
		ExternalId: "someExternalId", OrganizationId: domain.OrganizationId(uuid.New()),
		Tags: []domain.Tag{{Key: "tag100"}, {Key: "tag101"}}}

	model, err := state.sut.ParticipantCreate(state.ctx, request)
	assert.NoError(t, err)

	result, err := state.sut.ParticipantGet(state.ctx, model.Id)

	assert.NoError(t, err)
	assertParticipantsEqual(t, &request, &result.Participant)
}

func assertParticipantsEqual(t *testing.T, expected *domain.Participant, actual *domain.Participant) {
	assert.Equal(t, expected.ExternalId, actual.ExternalId)
	assert.Equal(t, expected.OrganizationId, actual.OrganizationId)
	assert.Equal(t, len(expected.Tags), len(actual.Tags))
	for i, tag := range expected.Tags {
		assert.Equal(t, tag, actual.Tags[i])
	}
}
