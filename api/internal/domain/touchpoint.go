package domain

// Some touchpoint eg Registration
type Touchpoint struct {
	Name string
}

type TouchpointModel struct {
	Model
	Touchpoint
}
