package legal

type Element interface {
	// todo can this either a provision or a subsection
}

type List struct {
	Elements []Element
}

type Checkbox struct {
	PermissionID string
	Required     bool // Todo depends on context a bit?
}

type Text struct {
	Content string
}
