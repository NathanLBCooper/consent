package legal

import "nathan.dev/consent/internal/domain"

// An collection of Sections forms a Document. Which is shown to Participants at the given touchpoints.
type Document struct {
	SectionIds    []string
	TouchpointIds []string
}

type DocumentModel struct {
	domain.Model
	Document
}
