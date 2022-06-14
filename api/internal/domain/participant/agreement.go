package participant

import "time"

type Agreement struct {
	ContractVersionId string
	PermissionId      string
	Accepted          bool
	AcceptedTime      time.Time
	// todo more optional context. Document, Touchpoint etc?
}
