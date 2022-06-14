package legal

import "time"

type Agreement struct {
	SectionVersionId string
	PermissionId     string
	Accepted         bool
	AcceptedTime     time.Time
}
