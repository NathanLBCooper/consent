package legal

type Element interface {
	// todo. Not sure burying the good stuff, permissions, in "Elements" works
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
