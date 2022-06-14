package account

import "nathan.dev/consent/internal/domain"

type User struct {
	Name string
}

type UserModel struct {
	domain.Model
	User
}
