package domain

type User struct {
	Name string
}

type UserModel struct {
	Model
	User
}

type Organization struct {
	Name    string
	Members map[string]*OrganizationMember
}

type OrganizationModel struct {
	Model
	Organization
}

type OrganizationMember struct {
	Role Role
}

type Role struct {
	slug string
}

func (r Role) String() string {
	return r.slug
}

var (
	Owner = Role{"owner"}
)
