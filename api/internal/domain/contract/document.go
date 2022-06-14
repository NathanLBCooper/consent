package contract

import "nathan.dev/consent/internal/domain"

// An collection of Contracts forms a Document. Which is shown to Participants at the given touchpoints.
type Document struct {
	ContractIds   []string
	TouchpointIds []string
}

type DocumentModel struct {
	domain.Model
	Document
}
