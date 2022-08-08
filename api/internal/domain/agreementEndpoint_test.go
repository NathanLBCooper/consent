package domain_test

import (
	"testing"
	"time"

	"github.com/google/uuid"
	"github.com/stretchr/testify/assert"
	"nathan.dev/consent/internal/domain"
	tc "nathan.dev/consent/internal/testcommon"
)

// Ignore versioning for first iteration
// todo, no checks that participants, permissions, contracts etc actually exist

type agreementTest struct {
	t   *testing.T
	ctx *tc.FakeContext
	sut *domain.AgreementEndpoint
}

func setupAgreementTest(t *testing.T) *agreementTest {
	repo := tc.NewFakeAgreementRepo()
	endpoint, err := domain.NewAgreementEndpoint(repo)
	assert.NoError(t, err)

	return &agreementTest{
		ctx: tc.NewFakeContext("someid"),
		sut: endpoint,
	}
}

func Test_can_accept_and_then_get_agreements(t *testing.T) {
	state := setupAgreementTest(t)
	agreements := [3]*domain.Agreement{
		agreementBuilder{}.build(),
		agreementBuilder{}.build(),
		agreementBuilder{Accepted: tc.NewPtr(false)}.build()}

	models := [len(agreements)]*domain.AgreementModel{}
	for i, ag := range agreements {
		model, err := state.sut.AgreementCreate(state.ctx, *ag)
		assert.NoError(t, err)
		models[i] = model
	}

	for i, a := range models {
		fetched, err := state.sut.AgreementGet(state.ctx, a.Id)
		assert.NoError(t, err)
		assertAgreementsEqual(t, agreements[i], &fetched.Agreement)
	}
}

func Test_can_get_all_agreements_for_a_participant(t *testing.T) {
	state := setupAgreementTest(t)

	participantId := domain.ParticipantId(uuid.New())
	participantsAgreements := []*domain.Agreement{
		agreementBuilder{ParticipantId: &participantId}.build(),
		agreementBuilder{ParticipantId: &participantId, Accepted: tc.NewPtr(true)}.build(),
		agreementBuilder{ParticipantId: &participantId, Accepted: tc.NewPtr(false)}.build()}

	participantAgreementModels := state.createAgreements(participantsAgreements)
	otherParticipant := domain.ParticipantId(uuid.New())
	otherAgreementModel := state.createAgreement(*agreementBuilder{ParticipantId: &otherParticipant}.build())

	result, err := state.sut.AgreementGetByParticipant(state.ctx, participantId)

	assert.NoError(t, err)
	assert.Equal(t, len(participantAgreementModels), len(result))
	for _, m := range result {
		assert.NotEqual(t, otherAgreementModel.Id, m.Id)
		var matching *domain.AgreementModel
		for _, a := range participantAgreementModels {
			if a.Id == m.Id {
				matching = a
				break
			}
		}
		assert.NotNil(t, matching)
		assertAgreementsEqual(t, &matching.Agreement, &m.Agreement)
	}
}

func Test_when_agreement_is_accepted_then_associated_permission_is_accepted(t *testing.T) {
	state := setupAgreementTest(t)
	expectedAccepted := true
	agreement := state.createAgreement(*agreementBuilder{Accepted: &expectedAccepted}.build())

	permissions, err := state.sut.GetPermissions(state.ctx, agreement.ParticipantId)
	assert.NoError(t, err)
	assert.Equal(t, 1, len(permissions))
	assert.Equal(t, agreement.PermissionId, permissions[0])

	accepted, err := state.sut.IsAccepted(state.ctx, agreement.ParticipantId, agreement.PermissionId)
	assert.NoError(t, err)
	assert.Equal(t, expectedAccepted, accepted)
}

func Test_when_a_permission_is_accepted_in_any_contract_it_is_accepted(t *testing.T) {
	state := setupAgreementTest(t)
	participant := domain.ParticipantId(uuid.New())
	permission := domain.PermissionId(uuid.New())

	yes := agreementBuilder{
		ParticipantId: &participant,
		PermissionId:  &permission,
		ContractId:    tc.NewPtr(domain.ContractId(uuid.New())),
		Accepted:      tc.NewPtr(true)}.build()
	newerNo := agreementBuilder{
		ParticipantId:        &participant,
		PermissionId:         &permission,
		ContractId:           tc.NewPtr(domain.ContractId(uuid.New())),
		Accepted:             tc.NewPtr(false),
		RelativeDecisionTime: tc.NewPtr(time.Hour)}.build()

	_ = state.createAgreement(*yes)
	_ = state.createAgreement(*newerNo)

	accepted, err := state.sut.IsAccepted(state.ctx, participant, permission)
	assert.NoError(t, err)
	assert.Equal(t, true, accepted)
}

func Test_newest_agreement_is_what_counts(t *testing.T) {
	state := setupAgreementTest(t)
	participant := domain.ParticipantId(uuid.New())
	permission := domain.PermissionId(uuid.New())
	contract := domain.ContractId(uuid.New())

	builder := agreementBuilder{
		ParticipantId: &participant,
		PermissionId:  &permission,
		ContractId:    &contract,
	}

	builder.Accepted = tc.NewPtr(true)
	yes_t0 := builder.build()
	_ = state.createAgreement(*yes_t0)

	accepted, err := state.sut.IsAccepted(state.ctx, participant, permission)
	assert.NoError(t, err)
	assert.Equal(t, true, accepted)

	builder.Accepted = tc.NewPtr(false)
	builder.RelativeDecisionTime = tc.NewPtr(2 * time.Second)
	no_t2 := builder.build()
	_ = state.createAgreement(*no_t2)
	builder.Accepted = tc.NewPtr(true)
	builder.RelativeDecisionTime = tc.NewPtr(time.Second)
	yes_t1 := builder.build()
	_ = state.createAgreement(*yes_t1)

	accepted, err = state.sut.IsAccepted(state.ctx, participant, permission)
	assert.NoError(t, err)
	assert.Equal(t, false, accepted)

	builder.Accepted = tc.NewPtr(true)
	builder.RelativeDecisionTime = tc.NewPtr(3 * time.Second)
	yes_t3 := builder.build()
	_ = state.createAgreement(*yes_t3)

	accepted, err = state.sut.IsAccepted(state.ctx, participant, permission)
	assert.NoError(t, err)
	assert.Equal(t, true, accepted)
}

func Test_agreements_that_have_been_superseeded_still_exist(t *testing.T) {
	state := setupAgreementTest(t)
	participant := domain.ParticipantId(uuid.New())
	permission := domain.PermissionId(uuid.New())
	contract := domain.ContractId(uuid.New())
	builder := agreementBuilder{
		ParticipantId: &participant,
		PermissionId:  &permission,
		ContractId:    &contract,
	}

	builder.Accepted = tc.NewPtr(false)
	no_t0 := builder.build()
	noModel_t0 := state.createAgreement(*no_t0)
	builder.Accepted = tc.NewPtr(true)
	yes_t1 := builder.build()
	_ = state.createAgreement(*yes_t1)

	getNo_t0, err := state.sut.AgreementGet(state.ctx, noModel_t0.Id)
	assert.NoError(t, err)
	assertAgreementsEqual(t, no_t0, &getNo_t0.Agreement)
}

func (a *agreementTest) createAgreement(agreement domain.Agreement) *domain.AgreementModel {
	model, err := a.sut.AgreementCreate(a.ctx, agreement)
	assert.NoError(a.t, err)
	return model
}

func (a *agreementTest) createAgreements(agreements []*domain.Agreement) []*domain.AgreementModel {
	var models []*domain.AgreementModel
	for _, ag := range agreements {
		models = append(models, a.createAgreement(*ag))
	}
	return models
}

type agreementBuilder struct {
	ParticipantId        *domain.ParticipantId
	ContractId           *domain.ContractId
	PermissionId         *domain.PermissionId
	RelativeDecisionTime *time.Duration
	Accepted             *bool
}

func (a agreementBuilder) build() *domain.Agreement {
	decisionTime := now()
	if a.RelativeDecisionTime != nil {
		decisionTime = decisionTime.Add(*a.RelativeDecisionTime)
	}
	return &domain.Agreement{
		ParticipantId: tc.Coalesce(a.ParticipantId, domain.ParticipantId(uuid.New())),
		ContractId:    tc.Coalesce(a.ContractId, domain.ContractId(uuid.New())),
		PermissionId:  tc.Coalesce(a.PermissionId, domain.PermissionId(uuid.New())),
		DecisionTime:  tc.Coalesce(&decisionTime, now()),
		Accepted:      tc.Coalesce(a.Accepted, true),
	}
}

func now() time.Time {
	return time.Date(2020, 05, 01, 9, 10, 15, 0, time.UTC)
}

func assertAgreementsEqual(t *testing.T, expected *domain.Agreement, actual *domain.Agreement) {
	assert.Equal(t, expected.ParticipantId, actual.ParticipantId)
	assert.Equal(t, expected.ContractId, actual.ContractId)
	assert.Equal(t, expected.PermissionId, actual.PermissionId)
	assert.Equal(t, expected.DecisionTime, actual.DecisionTime)
	assert.Equal(t, expected.Accepted, actual.Accepted)
}
