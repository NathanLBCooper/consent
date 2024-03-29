package domain

import "go.mongodb.org/mongo-driver/x/mongo/driver/uuid"

type Tag struct {
	Key string
}

type TagId uuid.UUID
type TagModel struct {
	Model[TagId]
	Tag
}

// todo I want to get Contracts for a Participant via their tags. Who is responsible?

// Thinking of having Tag un-normalised on both Contract and Participant
// but then having a store that I update from both of those and use that for autocomplete or adding stuff like a description
