package domain

import (
	"errors"
	"time"

	"github.com/google/uuid"
)

// Ignore versioning for first iteration

type Agreement struct {
	//ContractVersionId uuid.UUID
	ContractId    ContractId
	PermissionId  PermissionId
	ParticipantId ParticipantId
	DecisionTime  time.Time
	Accepted      bool
}

/*
Things will happen that make an agreement valid. Right now, a newer answer, but more things once versioning happens
How to deal with it? Maintain a table of all agreements ever (for audit), and a table of valid ones?

Since "auditablity" is at the core of what this need to do. Event store and read model?
Dual use of this. 1) What is accpeted now, and 2) what has ever happened. Maybe it doesn't make sense for one endpoint?
*/

type AgreementId uuid.UUID
type AgreementModel struct {
	Model[AgreementId]
	Agreement
}

type AgreementEndpoint struct {
}

func NewAgreementEndpoint() (*AgreementEndpoint, error) {
	return &AgreementEndpoint{}, nil
}

func (e *AgreementEndpoint) AgreementCreate(ctx Context, agreement Agreement) (*AgreementModel, error) {
	return nil, errors.New("not implemented")
}

func (e *AgreementEndpoint) AgreementGet(ctx Context, id AgreementId) (*AgreementModel, error) {
	return nil, errors.New("not implemented")
}

func (e *AgreementEndpoint) AgreementGetAll(ctx Context, participantId ParticipantId) ([]*AgreementModel, error) {
	return nil, errors.New("not implemented")
}

func (e *AgreementEndpoint) IsAccepted(ctx Context, participantId ParticipantId, permissionId PermissionId) (bool, error) {
	return false, errors.New("not implemented")
}

func (e *AgreementEndpoint) GetPermissions(ctx Context, participantId ParticipantId) ([]uuid.UUID, error) {
	return nil, errors.New("not implemented")
}
