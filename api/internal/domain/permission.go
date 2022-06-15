package domain

// Some specific idea that can be agreed to
type Permission struct {
	Name string
}

type PermissionModel struct {
	Model
	Permission
}
