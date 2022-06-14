package contract

import "nathan.dev/consent/internal/domain"

// Contains content that gathers (a) Permission(s).
// A self-contained unit that contains all wording required for informed consent
type Contract struct {
	Name     string
	Versions []*ContractVersion
}

type ContractModel struct {
	domain.Model
	Contract
}

type ContractVersion struct {
	Name   string
	Status ContractVersionStatus
	Body   Element
}

type ContractVersionStatus struct {
	slug string
}

func (r ContractVersionStatus) String() string {
	return r.slug
}

var (
	Draft      = ContractVersionStatus{"draft"}      // The only VersionStatus that is editable. Not shown to Participants
	Active     = ContractVersionStatus{"active"}     // Valid. Shown to Participants
	Legacy     = ContractVersionStatus{"legacy"}     // Still valid. Soft push Participants to active versions
	Deprecated = ContractVersionStatus{"deprecated"} // Still valid. Hard push Participants to active versions
	Obsolete   = ContractVersionStatus{"obsolete"}   // Is no longer valid. Hard push Participants to active versions
)
