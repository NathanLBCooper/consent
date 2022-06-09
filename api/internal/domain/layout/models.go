package layout

type Document struct {
	// Made up of multiple sections
}

type Section struct {
	Subsections []Subsection
	// the smallest unit of informed consent, contains text and agreements
}

type Subsection interface {
	// todo can this either a provision or a subsection
}

type Checkbox struct {
	ProvisionId string
}

type Text struct {
}
