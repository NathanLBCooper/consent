package legal

import "nathan.dev/consent/internal/domain"

// Contains content that gathers (a) Permission(s).
// A self-contained unit that contains all wording required for informed consent
type Section struct {
	Name     string
	Versions []SectionVersion
}

type SectionModel struct {
	domain.Model
	Section
}

type SectionVersion struct {
	Name   string
	Status VersionStatus
	Body   Element
}

type SectionVersionModel struct {
	domain.Model
	SectionVersion
}

type VersionStatus struct {
	slug string
}

func (r VersionStatus) String() string {
	return r.slug
}

var (
	Draft      = VersionStatus{"draft"}      // The only VersionStatus that is editable. Not shown to Participants
	Active     = VersionStatus{"active"}     // Valid. Shown to Participants
	Legacy     = VersionStatus{"legacy"}     // Still valid. Soft push Participants to active versions
	Deprecated = VersionStatus{"deprecated"} // Still valid. Hard push Participants to active versions
	Obsolete   = VersionStatus{"obsolete"}   // Is no longer valid. Hard push Participants to active versions
)
